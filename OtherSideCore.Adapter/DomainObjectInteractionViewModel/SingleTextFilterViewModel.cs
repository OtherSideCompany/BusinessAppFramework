using CommunityToolkit.Mvvm.ComponentModel;

namespace OtherSideCore.Adapter.DomainObjectInteraction
{
   public class SingleTextFilterViewModel : ObservableObject
   {
      #region Fields

      private string _filter;

      #endregion

      #region Properties

      public string Filter
      {
         get => _filter;
         set => SetProperty(ref _filter, value);
      }

      #endregion

      #region Events



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public SingleTextFilterViewModel()
      {       
         
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods




      #endregion
   }
}
