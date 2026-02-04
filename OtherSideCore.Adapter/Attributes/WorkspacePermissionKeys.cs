using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Adapter.Attributes
{
   [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
   public class WorkspacePermissionKeys : Attribute
   {
      public string[] Keys { get; }

      public WorkspacePermissionKeys(params string[] keys)
      {
         Keys = keys;
      }
   }
}
