namespace BusinessAppFramework.Application.Trees
{
    public class Node
    {
        #region Fields

        private readonly List<Branch> _branches = new();

        #endregion

        #region Properties

        public int Id { get; }
        public bool IsSelected { get; set; }
        public bool IsDirty { get; set; }
        public NodeSummary? Summary { get; set; }
        public List<Branch> ChildBranches { get; set; } = new();

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

        #endregion

        #region Private Methods



        #endregion
    }
}
