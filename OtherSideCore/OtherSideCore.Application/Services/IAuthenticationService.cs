using OtherSideCore.Domain.DomainObjects;
using System.Threading.Tasks;

namespace OtherSideCore.Application.Services
{
   public interface IAuthenticationService
   {
      Task<(bool, int)> VerifyPasswordAsync(string userName, string password);
   }
}
