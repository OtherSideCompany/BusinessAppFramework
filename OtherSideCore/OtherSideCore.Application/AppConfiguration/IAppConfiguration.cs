using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Application.AppConfiguration
{
   public interface IAppConfiguration
   {
      string ConfigFilePath { get; }
      bool RememberUserName { get; set; }
      string UserLogin { get; set; }

      void Load();
      void Save();
   }
}
