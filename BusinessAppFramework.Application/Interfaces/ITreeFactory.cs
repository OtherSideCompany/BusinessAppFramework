using BusinessAppFramework.Application.Trees;
using BusinessAppFramework.Domain;

namespace BusinessAppFramework.Application.Interfaces
{
   public interface ITreeFactory
   {
      void RegisterTree(StringKey key, Func<Tree> tree);
      Tree CreateTree(StringKey key);
   }
}
