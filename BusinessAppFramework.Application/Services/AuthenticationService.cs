using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Repository;
using BusinessAppFramework.Domain.Services;

namespace BusinessAppFramework.Application.Services
{
   public class AuthenticationService : IAuthenticationService
   {
      #region Fields

      protected IUserCredentialsRepository _userCredentialsRepository { get; set; }
      protected IPasswordService _passwordService { get; set; }

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public AuthenticationService(IUserCredentialsRepository userCredentialsRepository, IPasswordService passwordService)
      {
         _userCredentialsRepository = userCredentialsRepository;
         _passwordService = passwordService;
      }

      #endregion

      #region Public Methods

      public async Task<(bool, int)> VerifyPasswordAsync(string userName, string password)
      {
         (var userId, var passwordHash) = await _userCredentialsRepository.GetUserPasswordHashAsync(userName);

         if (_passwordService.VerifyPassword(passwordHash, password))
         {
            return (true, userId);
         }
         else
         {
            return (false, 0);
         }
      }

      #endregion

      #region Prvate Methods



      #endregion
   }
}
