using BusinessAppFramework.Application.Interfaces;

namespace BusinessAppFramework.Application.Trees
{
    public class Node
    {
        #region Fields


        #endregion

        #region Properties

        public int Id { get; set; }
        public int Depth { get; set; }
        public bool IsCyclic { get; set; }
        public bool IsExpanded { get; set; }
        public string TypeKey { get; set; } = default!;
        public NodeSummary? Summary { get; set; }
        public object? DomainObject { get; set; }
        public object? DomainObjectSearchResult { get; set; }
        public List<Branch> ChildBranches { get; set; } = new();
        public bool HasChildren { get => ChildBranches.Any(b => b.Nodes.Any()); }

        #endregion

        #region Events



        #endregion

        #region Constructor

        public Node(int id)
        {
            Id = id;
        }

        #endregion

        #region Public Methods

        public Branch? GetChildBranch(string parentChildRelationKey)
        {
            return ChildBranches.FirstOrDefault(b => b.ParentChildRelationKey.Equals(parentChildRelationKey));
        }

        public void ToggleExpanded()
        {
            IsExpanded = !IsExpanded;
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
