using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Adapter.Attributes
{
   [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
   public class DisplayKey : Attribute
   {
      public string Key { get; }

      public DisplayKey(string key)
      {
         Key = key;
      }
   }
}
