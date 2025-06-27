using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Adapter.Attributes
{
   [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
   public class EditorLabel : Attribute
   {
      public string Label { get; }

      public EditorLabel(string label)
      {
         Label = label;
      }
   }
}
