using System.Resources;

namespace OtherSideCore.Adapter.Services
{
    public interface ILocalizationService
    {
        void RegisterResourceManager(ResourceManager resourceManager);
        string GetString(string key);
    }
}
