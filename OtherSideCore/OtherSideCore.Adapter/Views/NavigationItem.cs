using CommunityToolkit.Mvvm.ComponentModel;
using OtherSideCore.Application;

namespace OtherSideCore.Adapter.Views
{
   public class NavigationItem : ObservableObject
   {
      #region Fields

      private bool _isSelected;

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
