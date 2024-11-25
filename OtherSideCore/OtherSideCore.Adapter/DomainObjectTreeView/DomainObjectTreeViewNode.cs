using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace OtherSideCore.Adapter.DomainObjectTreeView
{
   public class DomainObjectTreeViewNode : ObservableObject
   {
   
      #region Fields

      private DomainObjectViewModel _domainObjectViewModel;
      private DomainObjectTreeViewNode _parent;
      private ObservableCollection<DomainObjectTreeViewNode> _children;
      private DomainObjectTreeViewModel _domainObjectTreeViewModel;

      #endregion

      #region Properties

      public DomainObjectViewModel DomainObjectViewModel
      {
         get => _domainObjectViewModel;
         private set => SetProperty(ref _domainObjectViewModel, value);
      }

      public DomainObjectTreeViewNode Parent
      {
         get => _parent;
         private set => SetProperty(ref _parent, value);
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

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectTreeViewNode(DomainObjectViewModel domainObjectViewModel, DomainObjectTreeViewModel domainObjectTreeViewModel)
      {
         DomainObjectViewModel = domainObjectViewModel;
         Children = new ObservableCollection<DomainObjectTreeViewNode>();
         DomainObjectTreeViewModel = domainObjectTreeViewModel;
      }

      #endregion

      #region Public Methods

      public void AddChildNode(DomainObjectViewModel domainObjectViewModel)
      {
         var domainObjectTreeViewNode = new DomainObjectTreeViewNode(domainObjectViewModel, DomainObjectTreeViewModel);
         domainObjectTreeViewNode.Parent = this;
         Children.Add(domainObjectTreeViewNode);
      }

      public async Task SelectAsync()
      {
         await DomainObjectTreeViewModel.SelectDomainObjectViewModelAsync(DomainObjectViewModel);
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
