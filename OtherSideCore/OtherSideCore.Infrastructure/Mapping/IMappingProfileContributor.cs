using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Infrastructure.Mapping
{
   public interface IMappingProfileContributor
   {
      void ConfigureMap(Profile profile);
   }
}
