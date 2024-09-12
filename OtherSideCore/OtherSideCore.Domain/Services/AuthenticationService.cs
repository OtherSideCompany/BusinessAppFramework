using System.Threading.Tasks;
using OtherSideCore.Domain.ModelObjects;
using Microsoft.EntityFrameworkCore;
using CommunityToolkit.Mvvm.ComponentModel;
using OtherSideCore.Domain.Repositories;
using System.Threading;

namespace OtherSideCore.Domain.Services
{
   public class AuthenticationService<T> : ObservableObject, IAuthenticationService where T : User, new()
   {
      #region Fields

      private T m_AuthenticatedUser;
      private IRepositoryFactory _repositoryFactory;

      #endregion

      #region Properties

      public T AuthenticatedUser
      {
         get => m_AuthenticatedUser;
         private set => SetProperty(ref m_AuthenticatedUser, value);
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public AuthenticationService(IRepositoryFactory repositoryFactory)
      {
         _repositoryFactory = repositoryFactory;
      }

      #endregion

      #region Public Methods

      public bool IsUserAuthenticated()
      {
         return AuthenticatedUser != null;
      }

      public ModelObject GetAuthenticatedUser()
      {
         return AuthenticatedUser;
      }

      public bool CanAuthenticateUser(string userName, string password)
      {
         return !IsUserAuthenticated() && !string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password);
      }

      public async Task AuthenticateUserAsync(string userName, string password)
      {
         using var repository = _repositoryFactory.CreateUserRepository<T>();

         if (CanAuthenticateUser(userName, password))
         {
            (var userId, var passwordHash) = await repository.GetUserPasswordHashAsync(userName);

            if (userId > 0 && PasswordService.VerifyPassword(passwordHash, password))
            {
               AuthenticatedUser = await repository.GetAsync(userId, CancellationToken.None);
            }
         }
         else
         {
            AuthenticatedUser = null;
         }
      }

      public bool CanLogoutUser()
      {
         return IsUserAuthenticated();
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

      #endregion

      #region Prvate Methods



      #endregion
   }
}
