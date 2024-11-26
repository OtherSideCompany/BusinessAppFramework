using OtherSideCore.Appplication.Services;
using System.Collections.ObjectModel;

namespace OtherSideCore.Adapter.DomainObjectBrowser
{
   public class DomainObjectTreeViewModel : UIInteractionHost
   {
      #region Fields

      private IDomainObjectInteractionFactory _domainObjectInteractionFactory;

      private ObservableCollection<DomainObjectTreeViewNode> _roots;
      private DomainObjectViewModelSelection _selection;
      private bool _isSelectionLocked;
      private IDomainObjectEditorViewModel _selectedDomainObjectEditorViewModel;

      private IEnumerable<DomainObjectTreeViewNode> _inlineNodes
      {
         get
         {
            foreach (var node in _roots)
            {
               yield return node;

               foreach (var childNode in node.InlineNodes)
               {
                  yield return childNode;
               }
            }
         }
      }

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

      public IDomainObjectEditorViewModel SelectedDomainObjectEditorViewModel
      {
         get => _selectedDomainObjectEditorViewModel;
         private set => SetProperty(ref _selectedDomainObjectEditorViewModel, value);
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectTreeViewModel(IUserDialogService userDialogService,
                                       IWindowService windowService,
                                       IDomainObjectInteractionFactory domainObjectInteractionFactory) :
       base(userDialogService, windowService)
      {
         _domainObjectInteractionFactory = domainObjectInteractionFactory;

         Roots = new ObservableCollection<DomainObjectTreeViewNode>();
         Selection = new DomainObjectViewModelSelection(DomainObjectViewModelSelectionType.Single);
      }

      #endregion

      #region Public Methods

      public void AddRootNode(DomainObjectViewModel domainObjectViewModel)
      {
         var domainObjectRootTreeViewNode = _domainObjectInteractionFactory.CreateTreeViewNode(domainObjectViewModel, this);
         Roots.Add(domainObjectRootTreeViewNode);
      }

      public void AddChildNode(DomainObjectViewModel domainObjectViewModel, DomainObjectViewModel parentViewModel)
      {
         var domainObjectTreeViewNode = _domainObjectInteractionFactory.CreateTreeViewNode(domainObjectViewModel, this);
         var parentNode = GetNode(parentViewModel);
         domainObjectTreeViewNode.Parent = parentNode;
         parentNode.Children.Add(domainObjectTreeViewNode);
      }

      public async Task SelectNodeAsync(DomainObjectTreeViewNode domainObjectTreeViewNode)
      {
         if (!IsSelectionLocked)
         {
            if (ProceedSelection(domainObjectTreeViewNode.DomainObjectViewModel))
            {
               UnselectDomainObjectViewModel(domainObjectTreeViewNode.DomainObjectViewModel);

               if (domainObjectTreeViewNode.DomainObjectViewModel != null)
               {
                  Selection.SelectViewModel(domainObjectTreeViewNode.DomainObjectViewModel);
               }

               SelectedDomainObjectEditorViewModel = domainObjectTreeViewNode.DomainObjectEditorViewModel;

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

      private DomainObjectTreeViewNode GetNode(DomainObjectViewModel domainObjectViewModel)
      {
         return _inlineNodes.FirstOrDefault(node => node.DomainObjectViewModel.Equals(domainObjectViewModel));
      }

      private bool ProceedSelection(DomainObjectViewModel domainObjectViewModel)
      {
         return !domainObjectViewModel.Equals(Selection.SelectedViewModel);
      }

      #endregion
   }
}
