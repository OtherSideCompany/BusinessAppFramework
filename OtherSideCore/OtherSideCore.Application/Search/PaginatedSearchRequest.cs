using OtherSideCore.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Application.Search
{
    public class PaginatedSearchRequest : SearchRequest
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
