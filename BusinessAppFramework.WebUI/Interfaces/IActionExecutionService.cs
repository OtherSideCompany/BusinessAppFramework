using BusinessAppFramework.Application.ActionResult;
using BusinessAppFramework.Application.Interfaces;

namespace BusinessAppFramework.WebUI.Interfaces
{
   public interface IActionExecutionService
   {
      Task<DomainObjectApplicationActionResultPayload?> ExecuteAsync(IDomainObjectApplicationAction action);
   }
}
