namespace BusinessAppFramework.Application.Services
{
   public interface IUserContextService
   {
      Task<string?> GetUserNameAsync(int userId);
   }
}
