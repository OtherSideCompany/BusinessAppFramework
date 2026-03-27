using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAppFramework.Contracts.ApiRoutes
{
    public static class TreeRouteSegments
    {
        public const string CreateNode = $"create-node";
        public const string DeleteNode = $"delete-node";
        public const string GetTree = $"get-tree";
        public const string GetTreeBranch = $"get-tree-branch";
    }
}
