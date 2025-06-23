using OtherSideCore.Adapter.DomainObjectInteraction;
using OtherSideCore.Domain;
using OtherSideCore.Domain.DomainObjects;
using System.Collections.ObjectModel;

namespace OtherSideCore.Adapter.DomainObjectInteractionViewModel
{
   public static class DomainObjectTreeViewModelExtensions
   {
      #region Fields



      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion


      #region Public Methods

      public static void InsertNodeInList(IDomainObjectTreeNodeViewModel domainObjectTreeViewNode, ObservableCollection<IDomainObjectTreeNodeViewModel> nodes)
      {
         if (domainObjectTreeViewNode.DomainObjectViewModel.DomainObject is IIndexable indexableDomainObject)
         {
            if (!nodes.Any())
            {
               nodes.Add(domainObjectTreeViewNode);
            }
            else
            {
               var previousItems = nodes.Where(n => n.DomainObjectViewModel.DomainObject.GetType() == domainObjectTreeViewNode.DomainObjectViewModel.DomainObject.GetType() &&
                                                    ((IIndexable)n.DomainObjectViewModel.DomainObject).Index <= indexableDomainObject.Index);

               if (previousItems.Any())
               {
                  var previousItem = previousItems.OrderByDescending(c => ((IIndexable)c.DomainObjectViewModel.DomainObject).Index).First();
                  nodes.Insert(nodes.IndexOf(previousItem) + 1, domainObjectTreeViewNode);
               }
               else
               {
                  nodes.Add(domainObjectTreeViewNode);
               }
            }
         }
         else
         {
            nodes.Add(domainObjectTreeViewNode);
         }
      }

      public static void ConfigureTreeViewModel<TParent, TChild>(
        this DomainObjectTreeViewModel tree,
        Func<TParent, IEnumerable<TChild>> getChildren,
        Action<TParent, TChild> attachChild,
        Action<TParent, TChild> detachChild)
        where TParent : DomainObject
        where TChild : DomainObject
      {
         tree.DefaultRootType = typeof(TChild);

         tree.ResolveRootNodes = context =>
         {
            if (context.DomainObject is TParent parent)
            {
               return getChildren(parent).Cast<DomainObject>();
            }

            return Enumerable.Empty<DomainObject>();
         };

         tree.CreateRootDomainObjectAsync = async (type) => await tree.CreateTypedRootDomainObjectAsync(type);

         tree.AttachRootNode = (context, childViewModel) =>
         {
            if (context.DomainObject is TParent parent && childViewModel.DomainObject is TChild child)
            {
               attachChild(parent, child);
            }
         };

         tree.DetachRootNode = (context, childViewModel) =>
         {
            if (context.DomainObject is TParent parent && childViewModel.DomainObject is TChild child)
            {
               detachChild(parent, child);
            }
         };
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
