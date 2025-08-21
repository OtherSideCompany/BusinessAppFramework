using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Application.Services
{
   public interface IAuthenticationTokenService
   {
      string GenerateAccessToken(int userId);
      string GenerateRefreshToken();
   }
}
