using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Adapter.DomainObjectBrowser
{
   public class SearchParameters
   {
      public bool ExtendedSearch { get; set; }
      public DomainObjectViewModel? ParentViewModel { get; set; }
   }
}
