namespace BusinessAppFramework.Application.Repository
{
   public interface IUserCredentialsRepository
   {
        Task<bool> UserExistsAsync(string userName);
        Task<(int userId, string passwordHash)> GetUserPasswordHashAsync(string userName);
   }
}
