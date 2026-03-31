namespace BusinessAppFramework.Application.Interfaces
{
   public interface IAuthenticationTokenService
   {
      string GenerateAccessToken(int userId);
      string GenerateRefreshToken();
   }
}
