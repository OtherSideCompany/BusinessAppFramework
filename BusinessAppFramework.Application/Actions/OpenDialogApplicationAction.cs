using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Domain;

namespace BusinessAppFramework.Application.Actions
{
    public class OpenDialogApplicationAction : ApplicationAction, IOpenDialogApplicationAction
    {
        #region Fields



        #endregion

        #region Properties

        public StringKey ComponentKey { get; set; }
        public string DialogTitle { get; set; }

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
