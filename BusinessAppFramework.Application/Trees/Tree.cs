namespace BusinessAppFramework.Application.Trees
{
    public class Tree
    {
        #region Fields



        #endregion

        #region Properties

        public int RootId { get; set; }
        public object? Root { get; set; }
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

        #endregion

        #region Private Methods



        #endregion
    }
}
