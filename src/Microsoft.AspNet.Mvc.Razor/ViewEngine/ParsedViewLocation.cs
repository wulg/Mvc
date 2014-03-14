using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Microsoft.AspNet.Mvc.Razor
{
    public class ParsedViewLocation
    {
        private readonly string _formatString;
        private readonly Dictionary<string, int> _routeValuesToKeys = new Dictionary<string, int>();

        private enum ParseState
        {
            FreeText = 0,
            KeyName = 1
        }

        public ParsedViewLocation([NotNull]string viewLocation)
        {
            _formatString = ParseViewLocation(viewLocation);
        }

        private string ParseViewLocation(string viewLocation)
        {
            var formatStringBuilder = new StringBuilder();

            var state = ParseState.FreeText;
            int keyStartIndex = 0;
            int keyIndex = 0;
            int lastTextIndex = 0;

            for (int i = 0; i < viewLocation.Length; i++)
            {
                var ch = viewLocation[i];
                if (ch == '{')
                {
                    if (state == ParseState.KeyName)
                    {
                        ThrowInvalidViewLocation();
                    }

                    string freeText = viewLocation.Substring(lastTextIndex, i - lastTextIndex);
                    formatStringBuilder.Append(freeText);

                    state = ParseState.KeyName;
                    keyStartIndex = i + 1;
                }
                else if (ch == '}')
                {
                    if (state == ParseState.FreeText)
                    {
                        ThrowInvalidViewLocation();
                    }

                    state = ParseState.FreeText;

                    if (keyStartIndex == i)
                    {
                        ThrowInvalidViewLocation();
                    }

                    string keyText = "{" + keyIndex.ToString("D") + "}";
                    formatStringBuilder.Append(keyText);
                    lastTextIndex = i + 1;

                    string key = viewLocation.Substring(keyStartIndex, i - keyStartIndex);

                    if (!_routeValuesToKeys.ContainsKey(key))
                    {
                        _routeValuesToKeys.Add(key, keyIndex++);
                    }
                }
            }

            if (state == ParseState.KeyName)
            {
                ThrowInvalidViewLocation();
            }

            if (lastTextIndex < viewLocation.Length)
            {
                string trailingText = viewLocation.Substring(lastTextIndex);
                formatStringBuilder.Append(trailingText);
            }

            return formatStringBuilder.ToString();
        }

        private void ThrowInvalidViewLocation()
        {
            throw new ArgumentException("viewLocation is malformed, but have balanced {} with content and no nested {}", "viewLocation");
        }

        public string BuildPath([NotNull] KeyValuePair<string, string>[] routeValues)
        {
            var formatValues = new object[routeValues.Length];

            int count = 0;
            foreach (var kvp in routeValues)
            {
                int index;
                if (_routeValuesToKeys.TryGetValue(kvp.Key, out index))
                {
                    if (index < routeValues.Length)
                    {
                        formatValues[index] = kvp.Value;
                        count++;
                        continue;
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            if (count == _routeValuesToKeys.Count)
            {
                return string.Format(CultureInfo.InvariantCulture, _formatString, formatValues);
            }
            else
            {
                return null;
            }
        }
    }
}
