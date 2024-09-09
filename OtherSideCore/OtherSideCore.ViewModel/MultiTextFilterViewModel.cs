using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Domain;
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
      private AsyncRelayCommand m_SearchCommandAsync;

      #endregion

      #region Properties

      public MultiTextFilter MultiTextFilter
      {
         get => m_MultiTextFilter;
         set => SetProperty(ref m_MultiTextFilter, value);
      }

      public AsyncRelayCommand SearchCommandAsync
      {
         get => m_SearchCommandAsync;
         set => SetProperty(ref m_SearchCommandAsync, value);
      }

      #endregion

      #region Commands

      public RelayCommand AddFilterCommand { get; private set; }

      public RelayCommand<TextFilter> RemoveFilterCommand { get; private set; }

      #endregion

      #region Constructor

      public MultiTextFilterViewModel(MultiTextFilter multiTextFilter, AsyncRelayCommand searchCommandAsync)
      {
         AddFilterCommand = new RelayCommand(AddFilter);
         RemoveFilterCommand = new RelayCommand<TextFilter>(RemoveFilter, CanRemoveFilter);

         SearchCommandAsync = searchCommandAsync;

         MultiTextFilter = multiTextFilter;
      }

      #endregion

      #region Public Methods

      private void AddFilter()
      {
         MultiTextFilter.AddFilter();         
      }

      private bool CanRemoveFilter(TextFilter textFilter)
      {
         return textFilter != null;
      }

      private void RemoveFilter(TextFilter textFilter)
      {
         MultiTextFilter.RemoveFilter(textFilter);
      }

      public override void Dispose()
      {
         
      }

      #endregion
   }
}
