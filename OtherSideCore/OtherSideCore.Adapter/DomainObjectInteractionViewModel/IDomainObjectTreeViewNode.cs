using OtherSideCore.Domain.DomainObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Adapter.DomainObjectInteraction
{
   public interface IDomainObjectTreeViewNode : IDisposable
   {
      IEnumerable<IDomainObjectTreeViewNode> InlineNodes { get; }
      DomainObjectViewModel DomainObjectViewModel { get; }
      IDomainObjectEditorViewModel DomainObjectEditorViewModel { get; }
      ObservableCollection<IDomainObjectTreeViewNode> Children { get; }

      event EventHandler<IDomainObjectTreeViewNode> NodeSelectionRequested;
      event EventHandler<IDomainObjectTreeViewNode> NodeDeleted;
      event EventHandler<DomainObject> ChildCreated;

      void RequestSelection();
      void Select();
      void Unselect();
      void Expand();
      void Collapse();
      void AddChild(IDomainObjectTreeViewNode childNode, bool isInitializing);
      void RemoveChild(IDomainObjectTreeViewNode childNode);

   }
}
