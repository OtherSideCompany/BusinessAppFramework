namespace BusinessAppFramework.Application.Interfaces
{
   public interface IAuthenticationService
   {
      Task<(bool, int)> VerifyPasswordAsync(string userName, string password);
   }
}
