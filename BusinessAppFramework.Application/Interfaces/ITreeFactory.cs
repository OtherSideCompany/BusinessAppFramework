using BusinessAppFramework.Application.Trees;
using BusinessAppFramework.Domain;

namespace BusinessAppFramework.Application.Interfaces
{
   public interface ITreeFactory
   {
      void RegisterTree(string key, Func<Tree> tree);
      Tree CreateTree(string key);
   }
}
