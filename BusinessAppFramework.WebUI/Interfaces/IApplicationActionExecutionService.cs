using BusinessAppFramework.Application.Actions;
using BusinessAppFramework.Application.Interfaces;

namespace BusinessAppFramework.WebUI.Interfaces
{
   public interface IApplicationActionExecutionService
   {
        Task<DomainObjectApplicationActionResultPayload?> ExecuteApplicationActionAsync(IApplicationAction action);
   }
}
