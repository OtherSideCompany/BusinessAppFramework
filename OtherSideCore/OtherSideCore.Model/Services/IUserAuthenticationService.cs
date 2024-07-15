using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OtherSideCore.Model.ModelObjects;

namespace OtherSideCore.Model.Services
{
    public interface IUserAuthenticationService
   {
      User AuthenticatedUser { get; }

      bool CanAuthenticateUser();
      Task<User> AuthenticateUserAsync(string userName, string password);
      bool CanLogoutUser();
      Task<bool> LogoutUserAsync();
   }
}
