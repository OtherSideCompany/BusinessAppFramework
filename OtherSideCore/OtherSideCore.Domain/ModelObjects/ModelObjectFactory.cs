using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Domain.ModelObjects
{
   public class ModelObjectFactory : IModelObjectFactory
   {
      public virtual User CreateUser()
      {
         return new User();
      }
   }
}
