using BusinessAppFramework.Application.Interfaces;

namespace BusinessAppFramework.Application.Actions
{
    public class OpenDialogApplicationAction : ApplicationAction, IOpenDialogApplicationAction
    {
        #region Fields



        #endregion

        #region Properties

        public string ComponentKey { get; set; }
        public string DialogTitle { get; set; }
        public int? DomainObjectId { get; set; }

        #endregion

        #region Constructor

        public OpenDialogApplicationAction()
        {
            
        }

        #endregion

        #region Public Methods



        #endregion

        #region Private Methods



        #endregion

    }
}
