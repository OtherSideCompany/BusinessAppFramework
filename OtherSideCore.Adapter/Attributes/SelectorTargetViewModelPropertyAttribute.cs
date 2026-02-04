using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Adapter.Attributes
{
   [AttributeUsage(AttributeTargets.Property)]
   public class SelectorTargetViewModelPropertyAttribute : Attribute
   {
      public string TargetPropertyName { get; }

      public SelectorTargetViewModelPropertyAttribute(string targetPropertyName)
      {
         TargetPropertyName = targetPropertyName;
      }
   }
}
