namespace BusinessAppFramework.Application.Interfaces
{
    public interface ILocalizedStringService
    {
        void Add(string key, string culture, string value);
        string Get(string key);
        string Get(string key, string culture);
        void AddAggregate<T>(string cultureInfo, Dictionary<string, string> translations);
        string ResolveProperty<T>(string memberName);
        string ResolveProperty<T>(string memberName, string cultureInfo);
        string ResolveProperty(Type propertyType, string memberName);
        string ResolveProperty(Type propertyType, string memberName, string cultureInfo);
    }
}
