using BusinessAppFramework.Application.Actions;

namespace BusinessAppFramework.WebUI.Interfaces
{
   public interface IEditable
   {
      Task<List<DomainObjectApplicationActionResultPayload>> SaveChangesAsync();
      Task CancelChangesAsync();
   }
}
