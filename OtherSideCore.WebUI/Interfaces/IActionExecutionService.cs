using OtherSideCore.Application.ActionResult;
using OtherSideCore.Application.Interfaces;
using OtherSideCore.Contracts.ActionResult;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.WebUI.Interfaces
{
    public interface IActionExecutionService
    {
        Task<DomainObjectApplicationActionResultPayload?> ExecuteAsync(IDomainObjectApplicationAction action);
    }
}
