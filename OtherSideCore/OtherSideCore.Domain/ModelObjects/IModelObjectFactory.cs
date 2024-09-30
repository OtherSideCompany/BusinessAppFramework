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
      User CreateUser(IGlobalDataService globalDataService);
      T CreateModelObject<T>(IGlobalDataService globalDataService) where T : ModelObject, new();
   }
}
