using BusinessAppFramework.Application.Interfaces;

namespace BusinessAppFramework.Application.Actions
{
    public class FileDownloadApplicationAction : IFileDownloadApplicationAction
    {
        #region Properties

        public string ActionKey { get; init; } = string.Empty;
        public string ExecuteRoute { get; set; } = string.Empty;
        public string Route { get; init; } = string.Empty;

        #endregion

        #region Public Methods

        public string BuildRoute()
        {
            return Route;
        }

        #endregion
    }
}
