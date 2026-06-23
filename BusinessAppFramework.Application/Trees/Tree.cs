namespace BusinessAppFramework.Application.Trees
{
    public class Tree
    {
        #region Fields



        #endregion

        #region Properties

        public int RootId { get; set; }
        public List<Branch> Branches { get; set; } = new();

        #endregion

        #region Events



        #endregion

        #region Constructor

        public Tree()
        {

        }

        #endregion

        #region Public Methods

        public Branch? GetBranch(string parentChildRelationKey)
        {
            return Branches.FirstOrDefault(b => b.ParentChildRelationKey.Equals(parentChildRelationKey));
        }

        public void SetBranch(Branch branch)
        {
            var existing = Branches.FirstOrDefault(b => b.ParentChildRelationKey.Equals(branch.ParentChildRelationKey));

            if (existing is not null)
            {
                var index = Branches.IndexOf(existing);
                Branches[index] = branch;
            }
            else
            {
                Branches.Add(branch);
            }
        }

        public void ResetDepths()
        {
            ResetDepths(Branches, depth: 0);
        }      

        public void ExpandAll() => SetExpandedRecursive(Branches, true);

        public void CollapseAll() => SetExpandedRecursive(Branches, false);


        #endregion

        #region Private Methods

        private static void SetExpandedRecursive(IEnumerable<Branch> branches, bool isExpanded)
        {
            foreach (var branch in branches)
            {
                foreach (var node in branch.Nodes)
                {
                    node.IsExpanded = isExpanded;
                    SetExpandedRecursive(node.ChildBranches, isExpanded);
                }
            }
        }

        private static void ResetDepths(IEnumerable<Branch> branches, int depth)
        {
            foreach (var branch in branches)
            {
                foreach (var node in branch.Nodes)
                {
                    node.Depth = depth;
                    ResetDepths(node.ChildBranches, depth + 1);
                }
            }
        }

        #endregion
    }
}
