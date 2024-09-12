using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OtherSideCore.Domain.ModelObjects;

namespace OtherSideCore.Domain.Services
{
   public interface IAuthenticationService
   {
      ModelObject GetAuthenticatedUser();
      bool IsUserAuthenticated();
      bool CanAuthenticateUser(string userName, string password);
      Task AuthenticateUserAsync(string userName, string password);
      bool CanLogoutUser();
      Task<bool> LogoutUserAsync();
   }
}
