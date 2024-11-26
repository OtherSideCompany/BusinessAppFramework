using OtherSideCore.Appplication.Services;
using System.Collections.ObjectModel;

namespace OtherSideCore.Adapter.DomainObjectBrowser
{
   public class DomainObjectTreeViewNode : UIInteractionHost
   {

      #region Fields

      private IDomainObjectInteractionFactory _domainObjectInteractionFactory;

      private DomainObjectViewModel _domainObjectViewModel;
      private IDomainObjectEditorViewModel _domainObjectEditorViewModel;
      private DomainObjectTreeViewNode _parent;
      private ObservableCollection<DomainObjectTreeViewNode> _children;
      private DomainObjectTreeViewModel _domainObjectTreeViewModel;

      private IEnumerable<DomainObjectTreeViewNode> _inlineNodes
      {
         get
         {
            yield return this;

            if (Children.Any())
            {
               foreach (var childNode in InlineNodes)
               {
                  yield return childNode;
               }
            }
         }
      }

      #endregion

      #region Properties

      public DomainObjectViewModel DomainObjectViewModel
      {
         get => _domainObjectViewModel;
         private set => SetProperty(ref _domainObjectViewModel, value);
      }

      public IDomainObjectEditorViewModel DomainObjectEditorViewModel
      {
         get => _domainObjectEditorViewModel;
         private set => SetProperty(ref _domainObjectEditorViewModel, value);
      }

      public DomainObjectTreeViewNode Parent
      {
         get => _parent;
         set => SetProperty(ref _parent, value);
      }

      public ObservableCollection<DomainObjectTreeViewNode> Children
      {
         get => _children;
         private set => SetProperty(ref _children, value);
      }

      public DomainObjectTreeViewModel DomainObjectTreeViewModel
      {
         get => _domainObjectTreeViewModel;
         private set => SetProperty(ref _domainObjectTreeViewModel, value);
      }

      public IEnumerable<DomainObjectTreeViewNode> InlineNodes => _inlineNodes;

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectTreeViewNode(DomainObjectViewModel domainObjectViewModel,
                                      DomainObjectTreeViewModel domainObjectTreeViewModel,
                                      IUserDialogService userDialogService,
                                      IWindowService windowService,
                                      IDomainObjectInteractionFactory domainObjectInteractionFactory) :
         base(userDialogService, windowService)
      {
         _domainObjectInteractionFactory = domainObjectInteractionFactory;

         DomainObjectViewModel = domainObjectViewModel;
         DomainObjectEditorViewModel = domainObjectInteractionFactory.CreateDomainObjectEditorViewModel(domainObjectViewModel.DomainObject.GetType(), domainObjectViewModel);
         Children = new ObservableCollection<DomainObjectTreeViewNode>();
         DomainObjectTreeViewModel = domainObjectTreeViewModel;
      }

      #endregion

      #region Public Methods

      public async Task SelectAsync()
      {
         await DomainObjectTreeViewModel.SelectNodeAsync(this);
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
