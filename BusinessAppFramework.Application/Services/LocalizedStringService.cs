using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Contracts;
using System.Globalization;

namespace BusinessAppFramework.Application.Services
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

        public string Get(string key)
        {
            var currentCulture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            return Get(key, currentCulture);
        }

        public string Get(string key, string culture)
        {
            if (_data.TryGetValue(key, out var cultures))
            {
                if (cultures.TryGetValue(culture, out var value))
                    return value;
            }

            return key;
        }

        public void AddAggregate<T>(string culture, Dictionary<string, string> translations)
        {
            var prefix = typeof(T).Name;

            foreach (var (propertyName, value) in translations)
            {
                Add(propertyName, culture, value);
            }
        }

        public void AddEnum<TEnum>(TEnum value, string culture, string translation) where TEnum : struct, Enum
        {
            Add(EnumKeys<TEnum>.For(value), culture, translation);
        }

        public string ResolveProperty<T>(string memberName)
        {
            var cultureInfo = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            return ResolveProperty<T>(memberName, cultureInfo);
        }

        public string ResolveProperty<T>(string memberName, string cultureInfo)
        {
            return ResolveProperty(typeof(T), memberName, cultureInfo);
        }

        public string ResolveProperty(Type propertyType, string memberName)
        {
            var cultureInfo = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            return ResolveProperty(propertyType, memberName, cultureInfo);
        }

        public string ResolveProperty(Type propertyType, string memberName, string cultureInfo)
        {
            var type = propertyType;

            while (type is not null)
            {
                var key = AggregateKeys.Property(type, memberName);

                if (TryGet(cultureInfo, key, out var value))
                    return value;

                type = type.BaseType;
            }

            return memberName;
        }

        #region Private Methods

        private bool TryGet(string culture, string key, out string value)
        {
            if (_data.TryGetValue(key, out var cultures) && cultures.TryGetValue(culture, out value))
            {
                return true;
            }

            value = key;
            return false;
        }

        #endregion
    }
}
