using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.Framework.DependencyInjection;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Mvc.Razor.TagHelpers;
using System.IO;
using Microsoft.AspNet.Razor.TagHelpers;
using System.Threading.Tasks;

namespace Microsoft.AspNet.Mvc.Razor
{
    // TODO: Add an interface?
    // TODO: Name change?
    public class TagHelperRenderingHelper
    {
        private Stack<TagHelperRenderingContext> _renderingContexts;
        private MvcTagHelperContext _currentTagHelperContext;
        private TagHelperRenderingContext _currentRenderingContext;
        private IServiceProvider _serviceProvider;
        private ITypeActivator _typeActivator;
        private IDictionary<string, object> _attributeBuilders;

        public TagHelperRenderingHelper(IModelMetadataProvider metadataProvider, 
                                        IServiceProvider serviceProvider,
                                        ITypeActivator typeActivator)
	    {
            MetadataProvider = metadataProvider;
            // TODO: Pull these out into a tag helper activator
            _serviceProvider = serviceProvider;
            _typeActivator = typeActivator;
            _renderingContexts = new Stack<TagHelperRenderingContext>();
            _attributeBuilders = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }

        public IModelMetadataProvider MetadataProvider { get; private set; }
        public TextWriter TagBodyBuffer
        {
            get
            {
                return _currentRenderingContext.BodyBuffer;
            }
        }

        public void PrepareTagHelper(string tagName, ContentBehavior contentBehavior, ViewContext viewContext)
        {
            _currentRenderingContext = new TagHelperRenderingContext(new TagBuilder(tagName), contentBehavior);

            // If there's no valid body buffer for the current tag builder, inherit the previous one if there is one.
            if(_currentRenderingContext.BodyBuffer == null && _renderingContexts.Any())
            {
                _currentRenderingContext.BodyBuffer = _renderingContexts.Peek().BodyBuffer;
            }

            _renderingContexts.Push(_currentRenderingContext);

            _currentTagHelperContext = new MvcTagHelperContext(viewContext, MetadataProvider);
        }

        public async Task<string> StartTagHelper(Type[] tagHelperTypes)
        {
            foreach (var tagHelperType in tagHelperTypes)
            {
                var tagHelper = BuildTagHelper(tagHelperType);

                _currentRenderingContext.TagHelpers.Add(tagHelper);
            }

            var output = string.Empty;

            // Modify buffers the body so we wont want to call process here
            if (_currentRenderingContext.ContentBehavior != ContentBehavior.Modify)
            {
                await ProcessCurrentTagHelpers();

                if (_currentRenderingContext.ContentBehavior == ContentBehavior.Prepend)
                {
                    output = _currentRenderingContext.TagBuilder.ToString(TagRenderMode.Body);
                }
            }

            // Remove all tag attribute builders, no need to track them anymore
            _attributeBuilders.Clear();

            return _currentRenderingContext.TagBuilder.ToString(TagRenderMode.StartTag) + output;
        }

        public async Task<string> EndTagHelper()
        {
            _renderingContexts.Pop();

            string output = string.Empty;

            // The Replace content behavior is handled by the razor code generation piece.
            if (_currentRenderingContext.ContentBehavior != ContentBehavior.None && 
                _currentRenderingContext.ContentBehavior != ContentBehavior.Prepend)
            {
                if(_currentRenderingContext.ContentBehavior == ContentBehavior.Modify)
                {
                    // Process now because the TagBuilder's body will be populated
                    await ProcessCurrentTagHelpers();

                    // Build out the start tag here because the body was buffered
                    output = _currentRenderingContext.TagBuilder.ToString(TagRenderMode.StartTag);
                }

                output += _currentRenderingContext.TagBuilder.ToString(TagRenderMode.Body) + _currentRenderingContext.TagBuilder.ToString(TagRenderMode.EndTag);
            }
            else
            {
                output = _currentRenderingContext.TagBuilder.ToString(TagRenderMode.EndTag);
            }

            if (_renderingContexts.Any())
            {
                _currentRenderingContext = _renderingContexts.Peek();
            }

            return output;
        }

        // TODO: Is attribute builder the right name?
        public Task AddAttributeBuilder(string name, object obj)
        {
            _currentTagHelperContext.AttributeSource.Add(name, obj);
            _attributeBuilders.Add(name, obj);

            return Task.FromResult(result: true);
        }

        // TODO: Is attribute builder the right name?
        public async Task AddBufferedAttributeBuilder(string name, Func<TextWriter, Task<object>> valueBuilder)
        {
            var expression = await valueBuilder(new StringWriter());

            await AddAttributeBuilder(name, expression);
        }

        public async Task AddAttribute(string name, Func<TextWriter, Task<string>> valueBuilder)
        {
            var value = await valueBuilder(new StringWriter());

            _currentTagHelperContext.AttributeSource.Add(name, value);
            _currentRenderingContext.TagBuilder.Attributes.Add(name, value);
        }

        private async Task ProcessCurrentTagHelpers()
        {
            for (var i = 0; i < _currentRenderingContext.TagHelpers.Count; i++)
            {
                var helper = _currentRenderingContext.TagHelpers[i];

                await helper.ProcessAsync(_currentRenderingContext.TagBuilder, _currentTagHelperContext);
            }
        }

        private MvcTagHelper BuildTagHelper(Type tagHelperType)
        {
            var tagHelper = (MvcTagHelper)_typeActivator.CreateInstance(_serviceProvider, tagHelperType);
            // TODO: Cache these?
            var properties = tagHelperType.GetTypeInfo()
                                          .DeclaredProperties
                                          .Where(property => property.GetGetMethod() != null && property.GetSetMethod() != null);

            foreach(var property in properties)
            {
                var setter = PropertyHelper.MakeFastPropertySetter(property);

                if (_attributeBuilders.ContainsKey(property.Name))
                {
                    setter(tagHelper, _attributeBuilders[property.Name]);
                }
                else if(typeof(TagHelperExpression).IsAssignableFrom(property.PropertyType))
                {
                    setter(tagHelper, Activator.CreateInstance(property.PropertyType));
                }
            }

            return tagHelper;
        }
    }
}