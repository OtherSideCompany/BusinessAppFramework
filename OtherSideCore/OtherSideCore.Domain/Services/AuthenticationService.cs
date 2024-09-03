using System.Threading.Tasks;
using OtherSideCore.Domain.ModelObjects;
using Microsoft.EntityFrameworkCore;
using CommunityToolkit.Mvvm.ComponentModel;
using OtherSideCore.Domain.Repositories;

namespace OtherSideCore.Domain.Services
{
   public abstract class AuthenticationService<T> : ObservableObject, IAuthenticationService<T> where T : User, new()
   {
      #region Fields

      private User m_AuthenticatedUser;
      private Repositories.IRepositoryFactory _repositoryFactory;

      #endregion

      #region Properties

      public User AuthenticatedUser
      {
         get => m_AuthenticatedUser;
         private set => SetProperty(ref m_AuthenticatedUser, value);
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public AuthenticationService(Repositories.IRepositoryFactory repositoryFactory)
      {
         _repositoryFactory = repositoryFactory;
      }

      #endregion

      #region Methods

      public bool CanAuthenticateUser()
      {
         return AuthenticatedUser == null;
      }

      public async Task AuthenticateUserAsync(string userName, string passwordHash)
      {
         AuthenticatedUser = CanAuthenticateUser() ? await GetUserByCredentials(userName, passwordHash) : null;
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
         using var repository = _repositoryFactory.CreateUserRepository<T>();
         return await repository.GetSuperAdminUserAsync();
      }

      #endregion
   }
}
