using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Trees;

namespace BusinessAppFramework.Application.Factories
{
   public class TreeFactory : stringBasedFactory, ITreeFactory
   {
      #region Fields



      #endregion

      #region Properties



      #endregion

      #region Events



      #endregion

      #region Constructor

      public TreeFactory()
      {

      }

      #endregion

      #region Public Methods

      public Tree CreateTree(string key)
      {
         return (Tree)Create(key);
      }

      public void RegisterTree(string key, Func<Tree> tree)
      {
         Register(key, tree);
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
