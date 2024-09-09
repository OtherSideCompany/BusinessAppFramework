using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OtherSideCore.Domain.ModelObjects;

namespace OtherSideCore.Domain.Services
{
   public interface IAuthenticationService<T> where T : User
   {
      T AuthenticatedUser { get; }
      bool CanAuthenticateUser();
      Task AuthenticateUserAsync(string userName, string password);
      bool CanLogoutUser();
      Task<bool> LogoutUserAsync();
   }
}
