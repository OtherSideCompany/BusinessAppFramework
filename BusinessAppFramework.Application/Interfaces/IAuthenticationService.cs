namespace BusinessAppFramework.Application.Interfaces
{
   public interface IAuthenticationService
   {
        Task<bool> UserExistsAsync(string userName);
        Task<(bool, int)> VerifyPasswordAsync(string userName, string password);
   }
}
