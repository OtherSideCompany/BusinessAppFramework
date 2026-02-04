using Application.Trees;
using Domain;

namespace Application.Interfaces
{
   public interface ITreeFactory
   {
      void RegisterTree(StringKey key, Func<Tree> tree);
      Tree CreateTree(StringKey key);
   }
}
