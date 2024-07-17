using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.ViewModel
{
   public interface IDragDroppable
   {
      bool IsDropBeforeIndicatorVisible { get; set; }

      bool IsDropAfterIndicatorVisible { get; set; }

      void HideDropIndicators();
   }
}
