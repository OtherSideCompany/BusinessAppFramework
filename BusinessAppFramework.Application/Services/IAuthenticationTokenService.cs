namespace BusinessAppFramework.Application.Services
{
   public interface IAuthenticationTokenService
   {
      string GenerateAccessToken(int userId);
      string GenerateRefreshToken();
   }
}
