using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Adapter
{
   public interface IWindowSession
   {
      Task WhenClosed { get; }
   }
}
