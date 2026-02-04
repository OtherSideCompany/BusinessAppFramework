
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.Services
{
   public interface IUserContext
   {
      int Id { get; set; }
      string FirstName { get; set; }
      string LastName { get; set; }
      string UserName { get; set; }
      string GetName();
   }
}
