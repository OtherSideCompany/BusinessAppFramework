using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Domain.ModelObjects
{
   public interface IModelObjectFactory
   {
      public User CreateUser();
   }
}
