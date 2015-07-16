using System;
using System.Collections.Generic;

namespace CssSyntax
{
    public class CssSelectorContext : CssContext
    {
        private static readonly Char[] PropertyDelimiter = new[] { ';' };
        private static readonly Char PropertySeparator = ':';

        public CssSelectorContext(String selector) : base(selector) { }

        public Action<String, String> OnVisitProperty { get; set; }

        public override void Parse(String content)
        {
            foreach (var property in ParseProperties(content))
            {
                OnVisitProperty(property.Key, property.Value);
            }
        }

        private IEnumerable<KeyValuePair<String, String>> ParseProperties(String content)
        {
            var properties = content.Trim().Split(PropertyDelimiter, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < properties.Length; i++)
            {
                var parts = properties[i].Split(PropertySeparator);

                var key = parts[0].Trim();
                var value = parts[1].Trim();

                if (parts.Length > 2)
                {
                    for (var ip = 2; ip < parts.Length; ip++)
                    {
                        value += PropertySeparator;
                        value += parts[ip];
                    }
                }

                if (value.StartsWith("url(data"))
                {
                    value += PropertyDelimiter[0];
                    value += properties[++i];
                }

                yield return new KeyValuePair<String, String>(key, value);
            }
        }
    }
}