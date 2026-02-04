using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Domain.Services
{
   public interface IPasswordService
   {
      string HashPassword(string password);

      bool VerifyPassword(string storedHash, string providedPassword);
   }
}
