using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BusinessAppFramework.Domain.LocalizedTexts
{
    [JsonConverter(typeof(LocalizedTextJsonConverter))]
    public class LocalizedText
    {
        private readonly Dictionary<string, string> _values;

        public static LocalizedText Empty => new(new Dictionary<string, string>());

        public LocalizedText(IDictionary<string, string> values) => _values = new Dictionary<string, string>(values);

        public IReadOnlyDictionary<string, string> Values => _values;

        public string Resolve(string culture, string fallbackCulture) =>
            _values.TryGetValue(culture, out var value) ? value
            : _values.TryGetValue(fallbackCulture, out var fallback) ? fallback
            : string.Empty;

        public LocalizedText With(string culture, string value)
        {
            var updated = new Dictionary<string, string>(_values) { [culture] = value };
            return new LocalizedText(updated);
        }
    }
}
