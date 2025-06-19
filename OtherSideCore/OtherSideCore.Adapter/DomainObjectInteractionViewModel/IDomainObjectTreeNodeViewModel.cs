using OtherSideCore.Domain.DomainObjects;
using System.Collections.ObjectModel;

namespace OtherSideCore.Adapter.DomainObjectInteraction
{
   public interface IDomainObjectTreeNodeViewModel : IDisposable
   {
      IEnumerable<IDomainObjectTreeNodeViewModel> InlineNodes { get; }
      DomainObjectViewModel DomainObjectViewModel { get; }
      IDomainObjectEditorViewModel DomainObjectEditorViewModel { get; }
      ObservableCollection<IDomainObjectTreeNodeViewModel> Children { get; }

      event EventHandler<IDomainObjectTreeNodeViewModel> NodeSelectionRequested;
      event EventHandler<IDomainObjectTreeNodeViewModel> NodeDeleted;
      event EventHandler<DomainObject> ChildCreated;

      void RequestSelection();
      void Select();
      void Unselect();
      void Expand();
      void Collapse();
      void AddChild(IDomainObjectTreeNodeViewModel childNode, bool isInitializing);
      void RemoveChild(IDomainObjectTreeNodeViewModel childNode);
      Task<DomainObject> CreateChildNodeDomainObjectCopyAsync(IDomainObjectTreeNodeViewModel node);
      void NotifyChildCreated(DomainObject domainObject);
      Task InitializeAsync();
   }
}
