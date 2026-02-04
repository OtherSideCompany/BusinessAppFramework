namespace Domain.Services
{
   public interface IPasswordService
   {
      string HashPassword(string password);

      bool VerifyPassword(string storedHash, string providedPassword);
   }
}
