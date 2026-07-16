using BusinessAppFramework.Domain;

namespace BusinessAppFramework.Application.Interfaces
{
    public interface IApplicationAction
    {
        string ActionKey { get; }
        string ExecuteRoute { get; set; }
        string BuildRoute();
    }
}
