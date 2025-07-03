using OtherSideCore.Adapter.Views;
using OtherSideCore.Application;
using OtherSideCore.Application.Factories;
using OtherSideCore.Domain;

namespace OtherSideCore.Adapter.Factories
{
   public class WorkspaceFactory : StringKeyBasedFactory
   {
      #region Fields

      public StringKeyBasedFactory _factory;

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public WorkspaceFactory()
      {
         _factory = new StringKeyBasedFactory();
      }

      #endregion

      #region Public Methods

      public Workspace CreateWorkspace(StringKey key)
      {
         return (Workspace)Create(key);
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
