using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Model.ModelObjects
{
   public class ModelObjectFactory : IModelObjectFactory
   {
      public virtual User CreateUser()
      {
         return new User();
      }
   }
}
