namespace BusinessAppFramework.Application.Interfaces
{
   public interface IUserContextService
   {
      Task<string?> GetUserNameAsync(int userId);
   }
}
