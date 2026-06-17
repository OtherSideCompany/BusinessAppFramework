using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAppFramework.Application.Interfaces
{
    public interface IComposite<T> where T : IComposite<T>
    {
        public int ParentId { get; set; }
        public int Id { get; set; }
        public int Depth { get; set; }
        public bool HasChildren { get; set; }
        public IList<T> Children { get; set; }
        public bool IsCyclic { get; set; }
    }
}
