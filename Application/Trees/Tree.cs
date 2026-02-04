namespace Application.Trees
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

      #endregion

      #region Private Methods



      #endregion
   }
}
