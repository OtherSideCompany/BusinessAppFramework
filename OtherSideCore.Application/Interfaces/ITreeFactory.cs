using OtherSideCore.Application.Search;
using OtherSideCore.Application.Trees;
using OtherSideCore.Domain;
using System.Linq.Expressions;

namespace OtherSideCore.Application.Interfaces
{
    public interface ITreeFactory
    {
        void RegisterTree(StringKey key, Func<Tree> tree);
        Tree CreateTree(StringKey key);
    }
}
