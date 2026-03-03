using BusinessAppFramework.Domain;

namespace BusinessAppFramework.Application.Interfaces
{
    public interface IApplicationAction
    {
        StringKey ActionKey { get; }
        string ExecuteRouteTemplate { get; }
        string BuildRoute();
    }
}
