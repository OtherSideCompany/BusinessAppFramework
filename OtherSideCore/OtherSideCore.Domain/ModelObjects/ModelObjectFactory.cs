using CommunityToolkit.Mvvm.ComponentModel;
using OtherSideCore.Domain.Repositories;
using OtherSideCore.Domain.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Domain.ModelObjects
{
   public class ModelObjectFactory : ObservableObject, IModelObjectFactory
   {
      public virtual T CreateModelObject<T>(IGlobalDataService globalDataService) where T : ModelObject, new()
      {
         var modelObject = new T();
         modelObject.SetServices(this, globalDataService);
         modelObject.LoadDefaultProperties();

         return modelObject;
      }

      public virtual User CreateUser(IGlobalDataService globalDataService)
      {
         var user = new User();
         user.SetServices(this, globalDataService);
         user.LoadDefaultProperties();

         return user;
      }
   }
}
