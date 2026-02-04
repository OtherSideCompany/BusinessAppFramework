using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Trees;
using BusinessAppFramework.Domain;

namespace BusinessAppFramework.Application.Factories
{
   public class TreeFactory : StringKeyBasedFactory, ITreeFactory
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

      public Tree CreateTree(StringKey key)
      {
         return (Tree)Create(key);
      }

      public void RegisterTree(StringKey key, Func<Tree> tree)
      {
         Register(key, tree);
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
