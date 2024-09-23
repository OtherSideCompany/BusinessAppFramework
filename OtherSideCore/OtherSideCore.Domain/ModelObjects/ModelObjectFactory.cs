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
      public virtual User CreateUser()
      {
         return new User();
      }
   }
}
