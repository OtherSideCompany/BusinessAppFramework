using OtherSideCore.Domain.Repositories;
using OtherSideCore.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Domain.ModelObjects
{
   public interface IModelObjectFactory
   {
      User CreateUser(IModelObjectFactory modelObjectFactory, IGlobalDataService globalDataService);
   }
}
