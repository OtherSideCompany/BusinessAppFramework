using BusinessAppFramework.Domain;

namespace BusinessAppFramework.Application.Interfaces
{
    public interface IOpenDialogApplicationAction : IApplicationAction
    {
        StringKey ComponentKey { get; }
        string DialogTitle { get; }
    }
}
