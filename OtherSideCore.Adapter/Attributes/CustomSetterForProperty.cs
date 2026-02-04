using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Adapter.Attributes
{
   [AttributeUsage(AttributeTargets.Property)]
   public class CustomSetterForProperty : Attribute
   {
      public string TargetPropertyName { get; }

      public CustomSetterForProperty(string targetPropertyName)
      {
         TargetPropertyName = targetPropertyName;
      }
   }
}
