using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;

namespace OtherSideCore.Adapter.Views
{
   public class NavigationItem : ObservableObject
   {
      #region Fields

      private bool _isSelected;

      #endregion

      #region Properties

      public string Id { get; }
      public string Label { get; }
      public string IconKey { get; }
      public Type ViewModelType { get; }

      public bool IsSelected
      {
         get => _isSelected;
         set => SetProperty(ref _isSelected, value);
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public NavigationItem(string id, string label, string iconKey, Type viewModelType)
      {
         Id = id;
         Label = label;
         IconKey = iconKey;
         ViewModelType = viewModelType;
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
