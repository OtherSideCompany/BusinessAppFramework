using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OtherSideCore.Model.ModelObjects;

namespace OtherSideCore.Model.Services
{
    public abstract class UserAuthenticationService : IUserAuthenticationService
   {
      #region Fields

      private User m_AuthenticatedUser;

      #endregion

      #region Properties

      public User AuthenticatedUser
      {
         get => m_AuthenticatedUser;
         private set => m_AuthenticatedUser = value;
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public UserAuthenticationService()
      {

      }

      #endregion

      #region Methods

      public bool CanAuthenticateUser()
      {
         return AuthenticatedUser == null;
      }

      public async Task<User> AuthenticateUserAsync(string userName, string passwordHash)
      {
         return CanAuthenticateUser() ? await GetUserByCredentials(userName, passwordHash) : null;
      }

      public bool CanLogoutUser()
      {
         return AuthenticatedUser != null;
      }
      public async Task<bool> LogoutUserAsync()
      {
         if (CanLogoutUser())
         {
            AuthenticatedUser.Dispose();
            AuthenticatedUser = null;
            return true;
         }

         return false;
      }

      protected abstract Task<User> GetUserByCredentials(string userName, string passwordHash);

      #endregion
   }
}
