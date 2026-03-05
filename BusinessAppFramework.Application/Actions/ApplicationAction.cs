using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Domain;

namespace BusinessAppFramework.Application.Actions
{
    public abstract class ApplicationAction : IApplicationAction
    {
        #region Fields



        #endregion

        #region Properties

        public StringKey ActionKey { get; init; } = StringKey.Empty;
        public string ExecuteRoute { get; set; } = string.Empty;

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
            return ExecuteRoute;
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
