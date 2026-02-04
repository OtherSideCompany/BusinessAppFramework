namespace BusinessAppFramework.Application.Services
{
   public interface IAuthenticationService
   {
      Task<(bool, int)> VerifyPasswordAsync(string userName, string password);
   }
}
