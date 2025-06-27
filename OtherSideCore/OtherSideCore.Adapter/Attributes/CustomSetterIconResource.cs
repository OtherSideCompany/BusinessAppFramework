using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Adapter.Attributes
{
   [AttributeUsage(AttributeTargets.Property)]
   public class CustomSetterIconResource : Attribute
   {
      public string IconResource { get; }

      public CustomSetterIconResource(string iconResource)
      {
         IconResource = iconResource;
      }
   }
}
