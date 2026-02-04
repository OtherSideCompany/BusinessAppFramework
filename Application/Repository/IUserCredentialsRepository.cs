namespace Application.Repository
{
   public interface IUserCredentialsRepository
   {
      Task<(int userId, string passwordHash)> GetUserPasswordHashAsync(string userName);
   }
}
