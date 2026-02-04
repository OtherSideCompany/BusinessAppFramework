using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Adapter.Attributes
{
   [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
   public class DisplayIndex : Attribute
   {
      public int Index { get; }

      public DisplayIndex(int index)
      {
         Index = index;
      }
   }
}
