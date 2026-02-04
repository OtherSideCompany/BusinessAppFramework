using OtherSideCore.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace OtherSideCore.Application.Services
{
    public class LocalizedStringService : ILocalizedStringService
    {
        private readonly Dictionary<string, Dictionary<string, string>> _data = new(StringComparer.OrdinalIgnoreCase);

        public void Add(string key, string culture, string value)
        {
            if (!_data.TryGetValue(key, out var cultures))
            {
                cultures = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                _data[key] = cultures;
            }

            cultures[culture] = value;
        }

        public string? Get(string key)
        {
            var currentCulture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            return Get(key, currentCulture);
        }

        public string? Get(string key, string culture)
        {
            if (_data.TryGetValue(key, out var cultures))
            {
                if (cultures.TryGetValue(culture, out var value))
                    return value;
            }

            return key;
        }
    }
}
