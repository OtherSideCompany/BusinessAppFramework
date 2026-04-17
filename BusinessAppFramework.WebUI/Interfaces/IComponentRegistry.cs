using BusinessAppFramework.Domain;

namespace BusinessAppFramework.WebUI.Interfaces
{
    public interface IComponentRegistry
    {
        void Register(StringKey key, Type value);
        Type Resolve(StringKey key);
        bool TryResolve(StringKey key, out Type value);
    }
}
