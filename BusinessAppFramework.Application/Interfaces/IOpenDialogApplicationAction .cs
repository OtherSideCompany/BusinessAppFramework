using BusinessAppFramework.Domain;

namespace BusinessAppFramework.Application.Interfaces
{
    public interface IOpenDialogApplicationAction : IApplicationAction
    {
        Type ComponentType { get; }
        string DialogTitle { get; }
    }
}
