using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Adapter.DomainObjectBrowser
{
   public class PaginatedSearchParameters : SearchParameters
   {
      public bool ResetPage { get; set; }
   }
}
