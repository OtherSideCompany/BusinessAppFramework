using BusinessAppFramework.Domain;

namespace BusinessAppFramework.Application.Interfaces
{
    public interface IDomainObjectApplicationAction : IApplicationAction
    {
        int DomainObjectId { get; set; }
    }
}
