using OtherSideCore.Domain.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Application
{
   public class PagedResult<T> where T : DomainObject
   {
      public IEnumerable<T> Items { get; set; }
      public int TotalCount { get; set; }
      public int PageNumber { get; set; }
      public int PageSize { get; set; }
      public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
   }
}
