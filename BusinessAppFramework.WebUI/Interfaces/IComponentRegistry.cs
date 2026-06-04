using BusinessAppFramework.Domain;

namespace BusinessAppFramework.WebUI.Interfaces
{
    public interface IComponentRegistry
    {
        void Register(string key, Type value);
        Type Resolve(string key);
        bool TryResolve(string key, out Type value);
    }
}
