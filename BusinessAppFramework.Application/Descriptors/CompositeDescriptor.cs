using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAppFramework.Application.Descriptors
{
    public class CompositeDescriptor
    {
        public int Id { get; set; }
        public int Depth { get; set; }
        public bool HasChildren { get; set; }
        public List<int> ChildrenIds { get; set; } = new List<int>();
        public bool IsCyclic { get; set; }
    }
}
