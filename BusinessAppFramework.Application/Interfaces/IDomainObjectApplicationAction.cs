using BusinessAppFramework.Domain;

namespace BusinessAppFramework.Application.Interfaces
{
    public interface IDomainObjectApplicationAction
    {
        StringKey ActionKey { get; }
        string ExecuteRouteTemplate { get; }
        int? DomainObjectId { get; set; }
        bool RequireDomainObjectId { get; }
        string BuildRoute();
    }
}
