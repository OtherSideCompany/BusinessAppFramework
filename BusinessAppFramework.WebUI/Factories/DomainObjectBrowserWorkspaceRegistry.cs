using BusinessAppFramework.Application.Registry;
using BusinessAppFramework.Domain;
using BusinessAppFramework.Domain.DomainObjects;
using BusinessAppFramework.WebUI.Interfaces;

namespace BusinessAppFramework.WebUI.Factories
{
   public class DomainObjectBrowserWorkspaceRegistry : Registry<Type, string>, IDomainObjectBrowserWorkspaceRegistry
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

      public void Register<T>(string browserKey) where T : DomainObject
      {
         Register(typeof(T), browserKey);
      }

      public string Resolve<T>() where T : DomainObject
      {
         return Resolve(typeof(T));
      }

      public bool TryResolve<T>(out string browserKey) where T : DomainObject
      {
         return TryResolve(typeof(T), out browserKey);
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
