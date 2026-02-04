using BusinessAppFramework.Domain.Services;
using System;
using System.Security.Cryptography;

namespace BusinessAppFramework.Infrastructure.Password
{
   public class PasswordService : IPasswordService
   {
      private const int SaltSize = 16;
      private const int HashSize = 32;
      private const int Iterations = 10000;

      public string HashPassword(string password)
      {
         byte[] salt = new byte[SaltSize];
         using (var rng = RandomNumberGenerator.Create())
         {
            rng.GetBytes(salt);
         }

         var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
         byte[] hash = pbkdf2.GetBytes(HashSize);

         byte[] hashBytes = new byte[SaltSize + HashSize];
         Array.Copy(salt, 0, hashBytes, 0, SaltSize);
         Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

         return Convert.ToBase64String(hashBytes);
      }

      public bool VerifyPassword(string storedHash, string providedPassword)
      {
         if (string.IsNullOrEmpty(storedHash) || string.IsNullOrEmpty(providedPassword))
         {
            return false;
         }

         byte[] hashBytes = Convert.FromBase64String(storedHash);

         byte[] salt = new byte[SaltSize];
         Array.Copy(hashBytes, 0, salt, 0, SaltSize);

         byte[] storedHashBytes = new byte[HashSize];
         Array.Copy(hashBytes, SaltSize, storedHashBytes, 0, HashSize);

         var pbkdf2 = new Rfc2898DeriveBytes(providedPassword, salt, Iterations, HashAlgorithmName.SHA256);
         byte[] providedHashBytes = pbkdf2.GetBytes(HashSize);

         return CryptographicOperations.FixedTimeEquals(storedHashBytes, providedHashBytes);
      }
   }
}
