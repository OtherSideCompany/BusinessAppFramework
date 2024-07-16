using System.Threading.Tasks;
using OtherSideCore.Model.ModelObjects;
using Microsoft.EntityFrameworkCore;

namespace OtherSideCore.Model.Services
{
   public abstract class AuthenticationService<T, U, V> : IAuthenticationService<T> where T : User, new() 
                                                                                    where U : Data.Entities.User, new()
                                                                                    where V : DbContext
   {
      #region Fields

      private T m_AuthenticatedUser;
      private Repositories.UserRepository<T, U, V> _userRepository;

      #endregion

      #region Properties

      public T AuthenticatedUser
      {
         get => m_AuthenticatedUser;
         private set => m_AuthenticatedUser = value;
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public AuthenticationService(Repositories.UserRepository<T, U, V> userRepository)
      {
         _userRepository = userRepository;
      }

      #endregion

      #region Methods

      public bool CanAuthenticateUser()
      {
         return AuthenticatedUser == null;
      }

      public async Task<T> AuthenticateUserAsync(string userName, string passwordHash)
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

      protected async Task<T> GetUserByCredentials(string userName, string passwordHash)
      {
         return await _userRepository.GetSuperAdminUserAsync();
      }

      #endregion
   }
}
