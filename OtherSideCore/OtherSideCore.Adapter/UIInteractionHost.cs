using CommunityToolkit.Mvvm.ComponentModel;
using OtherSideCore.Appplication.Services;

namespace OtherSideCore.Adapter
{
    public class UIInteractionHost : ObservableObject
    {
        #region Fields

        protected IWindowService _windowService;
        protected IUserDialogService _userDialogService;

        #endregion

        #region Properties



        #endregion

        #region Commands



        #endregion

        #region Constructor

        public UIInteractionHost(IUserDialogService userDialogService, IWindowService windowService)
        {
            _userDialogService = userDialogService;
            _windowService = windowService;
        }

        #endregion

        #region Public Methods



        #endregion

        #region Private Methods
  

        #endregion
    }
}
