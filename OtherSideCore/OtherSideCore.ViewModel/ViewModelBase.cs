using OtherSideCore.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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

      private Command m_ToggleExpandCommand;

      #endregion

      #region Commands 

      public Command ToggleExpandCommand
      {
         get
         {
            if (m_ToggleExpandCommand == null)
            {
               m_ToggleExpandCommand = new Command(ExecuteToggleExpandCommand, CanExecuteToggleExpandCommand);
            }
            return m_ToggleExpandCommand;
         }
      }

      #endregion

      #region Properties

      public bool IsSelected
      {
         get
         {
            return m_IsSelected;
         }
         set
         {
            if (value != m_IsSelected)
            {
               m_IsSelected = value;
               OnPropertyChanged("IsSelected");
            }
         }
      }

      public bool IsVisible
      {
         get
         {
            return m_IsVisible;
         }
         set
         {
            if (value != m_IsVisible)
            {
               m_IsVisible = value;
               OnPropertyChanged("IsVisible");
            }
         }
      }

      public bool IsExpanded
      {
         get
         {
            return m_IsExpanded;
         }
         set
         {
            if (value != m_IsExpanded)
            {
               m_IsExpanded = value;
               OnPropertyChanged("IsExpanded");
            }
         }
      }

      public bool IsCollapsed
      {
         get
         {
            return m_IsCollapsed;
         }
         set
         {
            if (value != m_IsCollapsed)
            {
               m_IsCollapsed = value;
               OnPropertyChanged("IsCollapsed");
            }
         }
      }

      public bool IsDropBeforeIndicatorVisible
      {
         get
         {
            return m_IsDropBeforeIndicatorVisible;
         }
         set
         {
            if (value != m_IsDropBeforeIndicatorVisible)
            {
               m_IsDropBeforeIndicatorVisible = value;
               OnPropertyChanged("IsDropBeforeIndicatorVisible");
            }
         }
      }

      public bool IsDropAfterIndicatorVisible
      {
         get
         {
            return m_IsDropAfterIndicatorVisible;
         }
         set
         {
            if (value != m_IsDropAfterIndicatorVisible)
            {
               m_IsDropAfterIndicatorVisible = value;
               OnPropertyChanged("IsDropAfterIndicatorVisible");
            }
         }
      }

      #endregion

      #region Constructor

      public ViewModelBase()
      {
         
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

      protected virtual bool CanExecuteToggleExpandCommand(object parameter)
      {
         return true;
      }

      protected virtual void ExecuteToggleExpandCommand(object parameter)
      {
         IsExpanded = !IsExpanded;
      }      

      public abstract void Dispose();

      #endregion

   }
}
