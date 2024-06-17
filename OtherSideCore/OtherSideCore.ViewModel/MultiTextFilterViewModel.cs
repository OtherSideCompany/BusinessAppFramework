using OtherSideCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace OtherSideCore.ViewModel
{
   public class MultiTextFilterViewModel : ViewModelBase
   {
      #region Fields

      private MultiTextFilter m_MultiTextFilter;

      private Command m_AddFilterCommand;
      private Command m_RemoveFilterCommand;

      #endregion

      #region Properties

      public MultiTextFilter MultiTextFilter
      {
         get
         {
            return m_MultiTextFilter;
         }
         set
         {
            if (value != m_MultiTextFilter)
            {
               m_MultiTextFilter = value;
               OnPropertyChanged(nameof(MultiTextFilter));
            }
         }
      }

      #endregion

      #region Commands

      public Command AddFilterCommand
      {
         get
         {
            if (m_AddFilterCommand == null)
            {
               m_AddFilterCommand = new Command(ExecuteAddFilterCommand, CanExecuteAddFilterCommand);
            }

            return m_AddFilterCommand;
         }
      }

      public Command RemoveFilterCommand
      {
         get
         {
            if (m_RemoveFilterCommand == null)
            {
               m_RemoveFilterCommand = new Command(ExecuteRemoveFilterCommand, CanExecuteRemoveFilterCommand);
            }

            return m_RemoveFilterCommand;
         }
      }

      #endregion

      #region Constructor

      public MultiTextFilterViewModel(MultiTextFilter multiTextFilter)
      {
         MultiTextFilter = multiTextFilter;
      }

      #endregion

      #region Methods

      private bool CanExecuteAddFilterCommand(object parameter)
      {
         return true;
      }

      private void ExecuteAddFilterCommand(object parameter)
      {
         MultiTextFilter.AddFilter();         
      }

      private bool CanExecuteRemoveFilterCommand(object parameter)
      {
         return parameter as TextFilter != null;
      }

      private void ExecuteRemoveFilterCommand(object parameter)
      {
         var filter = parameter as TextFilter;
         MultiTextFilter.RemoveFilter(filter);
      }

      public override void Dispose()
      {
         
      }

      #endregion
   }
}
