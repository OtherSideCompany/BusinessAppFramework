using CommunityToolkit.Mvvm.ComponentModel;
using OtherSideCore.Adapter.DomainObjectBrowser;
using System.Collections.ObjectModel;

namespace OtherSideCore.Adapter.DomainObjectTreeView
{
   public class DomainObjectTreeViewModel : ObservableObject
   {
      #region Fields

      private ObservableCollection<DomainObjectTreeViewNode> _roots;
      private DomainObjectViewModelSelection _selection;
      private bool _isSelectionLocked;

      #endregion

      #region Properties

      public ObservableCollection<DomainObjectTreeViewNode> Roots
      {
         get => _roots;
         private set => SetProperty(ref _roots, value);
      }

      public DomainObjectViewModelSelection Selection
      {
         get => _selection;
         private set => SetProperty(ref _selection, value);
      }

      public bool IsSelectionLocked
      {
         get => _isSelectionLocked;
         private set => SetProperty(ref _isSelectionLocked, value);
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectTreeViewModel()
      {
         Roots = new ObservableCollection<DomainObjectTreeViewNode>();
         Selection = new DomainObjectViewModelSelection(DomainObjectViewModelSelectionType.Single);
      }

      #endregion

      #region Public Methods

      public void AddRootNode(DomainObjectTreeViewNode rootNode)
      {
         Roots.Add(rootNode);
      }

      public async Task SelectDomainObjectViewModelAsync(DomainObjectViewModel domainObjectViewModel)
      {
         if (!IsSelectionLocked)
         {
            if (ProceedSelection(domainObjectViewModel))
            {
               UnselectDomainObjectViewModel(domainObjectViewModel);

               if (domainObjectViewModel != null)
               {
                  Selection.SelectViewModel(domainObjectViewModel);
               }

               //OnPropertyChanged(nameof(SelectedDomainObjectEditorViewModel));
               //NotifyCommandsCanExecuteChanged();

               //IsLoadingNestedBrowsers = true;

               //await LoadNestedBrowsersAsync();

               //foreach (var nestedBrowserViewModel in InlineNestedDomainObjectBrowserViewModels)
               //{
               //   await nestedBrowserViewModel.LoadNestedBrowsersAsync();
               //   nestedBrowserViewModel.PropertyChanged += NestedDomainObjectBrowserViewModel_PropertyChanged;
               //}

               //IsLoadingNestedBrowsers = false;
            }
         }
      }

      public void UnselectDomainObjectViewModel(DomainObjectViewModel domainObjectViewModel)
      {
         if (!IsSelectionLocked)
         {
            if (domainObjectViewModel != null)
            {
               Selection.UnselectViewModel(domainObjectViewModel);
            }

            //foreach (var nestedBrowserViewModel in InlineNestedDomainObjectBrowserViewModels)
            //{
            //   nestedBrowserViewModel.PropertyChanged -= NestedDomainObjectBrowserViewModel_PropertyChanged;
            //}

            //OnPropertyChanged(nameof(SelectedDomainObjectEditorViewModel));
            //NotifyCommandsCanExecuteChanged();
         }
      }

      #endregion

      #region Private Methods

      private bool ProceedSelection(DomainObjectViewModel domainObjectViewModel)
      {
         return !domainObjectViewModel.Equals(Selection.SelectedViewModel);
      }

      #endregion
   }
}
