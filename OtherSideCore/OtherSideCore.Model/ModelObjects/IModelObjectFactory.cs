using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Model.ModelObjects
{
   public interface IModelObjectFactory
   {
      public User CreateUser();
   }
}
