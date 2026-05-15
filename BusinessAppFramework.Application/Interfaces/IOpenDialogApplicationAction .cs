using BusinessAppFramework.Domain;

namespace BusinessAppFramework.Application.Interfaces
{
    public interface IOpenDialogApplicationAction : IApplicationAction
    {
        string ComponentKey { get; }
        string DialogTitle { get; }
        int? DomainObjectId { get; set; }
    }
}
