using Application.Registry;
using Domain;
using Domain.DomainObjects;
using WebUI.Interfaces;

namespace WebUI.Factories
{
   public class DomainObjectBrowserWorkspaceRegistry : Registry<Type, StringKey>, IDomainObjectBrowserWorkspaceRegistry
   {
      #region Fields



      #endregion

      #region Properties



      #endregion

      #region Events



      #endregion

      #region Constructor

      public DomainObjectBrowserWorkspaceRegistry()
      {

      }


      #endregion

      #region Public Methods

      public void Register<T>(StringKey browserKey) where T : DomainObject
      {
         Register(typeof(T), browserKey);
      }

      public StringKey Resolve<T>() where T : DomainObject
      {
         return Resolve(typeof(T));
      }

      public bool TryResolve<T>(out StringKey browserKey) where T : DomainObject
      {
         return TryResolve(typeof(T), out browserKey);
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
