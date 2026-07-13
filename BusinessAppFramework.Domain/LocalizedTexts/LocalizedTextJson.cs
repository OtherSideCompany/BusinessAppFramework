using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace BusinessAppFramework.Domain.LocalizedTexts
{
    public static class LocalizedTextJson
    {
        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public static string Serialize(LocalizedText localizedText) =>
            JsonSerializer.Serialize(localizedText.Values, SerializerOptions);

        public static LocalizedText Deserialize(string json) =>
            string.IsNullOrWhiteSpace(json)
                ? LocalizedText.Empty
                : new LocalizedText(
                    JsonSerializer.Deserialize<Dictionary<string, string>>(json, SerializerOptions)
                    ?? new Dictionary<string, string>());
    }
}
