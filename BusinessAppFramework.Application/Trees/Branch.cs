using System.Text.Json.Serialization;

namespace BusinessAppFramework.Application.Trees
{
    public class Branch
    {
        #region Fields



        #endregion

        #region Properties

        public bool IsExpanded { get; set; }
        public List<Node> Nodes { get; set; } = new();
        public string ParentChildRelationKey { get; set; } = string.Empty;
        public bool IsSelfReferencing { get; set; }

        [JsonIgnore]
        public List<Branch> ChildBranchTemplates { get; set; } = new();

        #endregion

        #region Events



        #endregion

        #region Constructor

        public Branch()
        {

        }

        public Branch(string parentChildRelationKey)
        {
            ParentChildRelationKey = parentChildRelationKey;
        }

        public Branch(Branch from)
        {
            ParentChildRelationKey = from.ParentChildRelationKey;
            ChildBranchTemplates = from.ChildBranchTemplates;
            IsSelfReferencing = from.IsSelfReferencing;
        }

        #endregion

        #region Public Methods       

        public void AddNode(Node n) => Nodes.Add(n);

        public void RemoveNode(int id) => Nodes.RemoveAll(n => n.Id == id);

        public void ClearNodes() => Nodes.Clear();

        #endregion

        #region Private Methods



        #endregion
    }
}
