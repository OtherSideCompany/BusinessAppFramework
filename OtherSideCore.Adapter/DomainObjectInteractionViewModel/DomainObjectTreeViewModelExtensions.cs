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
        this DomainObjectTreeViewModel treeViewModel)
        where TParent : DomainObject
        where TChild : DomainObject, new()
      {
         treeViewModel.DefaultRootType = typeof(TChild);

         treeViewModel.ResolveRootNodesAsync = async context =>
         {
            if (context.DomainObject is TParent parent)
            {
               var domainObjectService = treeViewModel.DomainObjectTreeViewModelDependencies.DomainObjectServiceFactory.CreateDomainObjectService<TChild>();
               return (await domainObjectService.TryGetAllAsync(parent)).Items;
            }

            return Enumerable.Empty<DomainObject>();
         };

         treeViewModel.CreateRootDomainObjectAsync = async (type) => await treeViewModel.CreateTypedRootDomainObjectAsync(type);
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
