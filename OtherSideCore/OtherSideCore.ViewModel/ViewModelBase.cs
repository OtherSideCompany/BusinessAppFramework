using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace OtherSideCore.ViewModel
{
   public abstract class ViewModelBase : ObservableObject, IDisposable, IDragDroppable
   {
      #region Fields

      private bool m_IsSelected;
      private bool m_IsVisible = true;
      private bool m_IsExpanded;
      private bool m_IsCollapsed;

      private bool m_IsDropBeforeIndicatorVisible;
      private bool m_IsDropAfterIndicatorVisible;

      #endregion

      #region Commands 

      public RelayCommand ToggleExpandCommand { get; private set; }

      #endregion

      #region Properties

      public bool IsSelected
      {
         get => m_IsSelected;
         set => SetProperty(ref m_IsSelected, value);
      }

      public bool IsVisible
      {
         get => m_IsVisible;
         set => SetProperty(ref m_IsVisible, value);
      }

      public bool IsExpanded
      {
         get => m_IsExpanded;
         set => SetProperty(ref m_IsExpanded, value);
      }

      public bool IsCollapsed
      {
         get => m_IsCollapsed;
         set => SetProperty(ref m_IsCollapsed, value);
      }

      public bool IsDropBeforeIndicatorVisible
      {
         get => m_IsDropBeforeIndicatorVisible;
         set => SetProperty(ref m_IsDropBeforeIndicatorVisible, value);
      }

      public bool IsDropAfterIndicatorVisible
      {
         get => m_IsDropAfterIndicatorVisible;
         set => SetProperty(ref m_IsDropAfterIndicatorVisible, value);
      }

      #endregion

      #region Constructor

      public ViewModelBase()
      {
         ToggleExpandCommand = new RelayCommand(ExecuteToggleExpandCommand);
      }

      #endregion

      #region Static Methods



      #endregion

      #region Methods

      public void HideDropIndicators()
      {
         IsDropAfterIndicatorVisible = false;
         IsDropBeforeIndicatorVisible = false;
      }

      protected virtual void ExecuteToggleExpandCommand()
      {
         IsExpanded = !IsExpanded;
      }      

      public abstract void Dispose();

      #endregion

   }
}
