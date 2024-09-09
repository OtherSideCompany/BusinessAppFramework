using System.Threading.Tasks;
using OtherSideCore.Domain.ModelObjects;
using Microsoft.EntityFrameworkCore;
using CommunityToolkit.Mvvm.ComponentModel;
using OtherSideCore.Domain.Repositories;

namespace OtherSideCore.Domain.Services
{
   public class AuthenticationService<T> : ObservableObject, IAuthenticationService<T> where T : User, new()
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

      public bool CanAuthenticateUser()
      {
         return AuthenticatedUser == null;
      }

      public async Task AuthenticateUserAsync(string userName, string passwordHash)
      {
         using var repository = _repositoryFactory.CreateUserRepository<T>();
         AuthenticatedUser = CanAuthenticateUser() ? await repository.GetUserByCredentials(userName, passwordHash) : null;
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

      #endregion

      #region Prvate Methods



      #endregion
   }
}
