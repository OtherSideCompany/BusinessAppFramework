using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Contracts;
using BusinessAppFramework.Domain;

namespace BusinessAppFramework.Application.Actions
{
    public abstract class ApplicationAction : IApplicationAction
    {
        #region Fields



        #endregion

        #region Properties

        public StringKey ActionKey { get; init; } = StringKey.Empty;
        public string ExecuteRouteTemplate { get; init; } = string.Empty;
        public string ControllerName { get; set;  } = string.Empty;

        #endregion

        #region Events



        #endregion

        #region Constructor

        public ApplicationAction()
        {

        }

        #endregion

        #region Public Methods

        public virtual string BuildRoute()
        {
            return Routes.BuildControllerRoute(ExecuteRouteTemplate, ControllerName);
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
