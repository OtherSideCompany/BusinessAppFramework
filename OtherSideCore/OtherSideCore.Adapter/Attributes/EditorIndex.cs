using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Adapter.Attributes
{
   [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
   public class EditorIndex : Attribute
   {
      public int Index { get; }

      public EditorIndex(int index)
      {
         Index = index;
      }
   }
}
