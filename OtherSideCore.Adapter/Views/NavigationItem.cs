using CommunityToolkit.Mvvm.ComponentModel;
using OtherSideCore.Domain;

namespace OtherSideCore.Adapter.Views
{
   public class NavigationItem : ObservableObject
   {
      #region Fields

      private bool _isSelected;
      private bool _isVisible;

      #endregion

      #region Properties

      public StringKey Key { get; }
      public string Label { get; }
      public string IconKey { get; }
      public Type ViewModelType { get; }

      public bool IsSelected
      {
         get => _isSelected;
         set => SetProperty(ref _isSelected, value);
      }

      public bool IsVisible
      {
         get => _isVisible;
         set => SetProperty(ref _isVisible, value);
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public NavigationItem(StringKey id, string label, string iconKey, Type viewModelType)
      {
         Key = id;
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
