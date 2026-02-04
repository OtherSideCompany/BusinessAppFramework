using Application.ActionResult;
using Application.Interfaces;

namespace WebUI.Interfaces
{
   public interface IActionExecutionService
   {
      Task<DomainObjectApplicationActionResultPayload?> ExecuteAsync(IDomainObjectApplicationAction action);
   }
}
