using CommunityToolkit.Mvvm.Input;
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

      #endregion

      #region Properties

      public MultiTextFilter MultiTextFilter
      {
         get => m_MultiTextFilter;
         set => SetProperty(ref m_MultiTextFilter, value);
      }

      #endregion

      #region Commands

      public RelayCommand AddFilterCommand { get; private set; }

      public RelayCommand<TextFilter> RemoveFilterCommand { get; private set; }

      #endregion

      #region Constructor

      public MultiTextFilterViewModel(MultiTextFilter multiTextFilter)
      {
         AddFilterCommand = new RelayCommand(ExecuteAddFilterCommand);
         RemoveFilterCommand = new RelayCommand<TextFilter>(ExecuteRemoveFilterCommand, CanExecuteRemoveFilterCommand);

         MultiTextFilter = multiTextFilter;
      }

      #endregion

      #region Methods

      private void ExecuteAddFilterCommand()
      {
         MultiTextFilter.AddFilter();         
      }

      private bool CanExecuteRemoveFilterCommand(TextFilter textFilter)
      {
         return textFilter != null;
      }

      private void ExecuteRemoveFilterCommand(TextFilter textFilter)
      {
         MultiTextFilter.RemoveFilter(textFilter);
      }

      public override void Dispose()
      {
         
      }

      #endregion
   }
}
