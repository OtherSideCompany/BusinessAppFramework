using OtherSideCore.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Application.Search
{
    public class SearchRequest
    {
        public bool ExtendedSearch { get; set; }
        public List<string> Filters { get; set; } = [];
        public string ConstraintKey { get; set; } = Contracts.ConstraintKeys.AllConstraintKey;
    }
}
