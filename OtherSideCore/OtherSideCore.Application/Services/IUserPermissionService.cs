using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.Services
{
   public interface IUserPermissionService
   {
      bool CanCreate<T>() where T : DomainObject;
      bool CanUpdate<T>() where T : DomainObject;
      bool CanDelete<T>() where T : DomainObject;
      bool CanRead<T>() where T : DomainObject;
   }
}
