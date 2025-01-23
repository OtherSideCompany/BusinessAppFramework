using OtherSideCore.Adapter.DomainObjectInteraction;
using OtherSideCore.Domain;
using System.Collections.ObjectModel;

namespace OtherSideCore.Adapter.DomainObjectInteractionViewModel
{
   public static class DomainObjectTreeViewModelExtension
   {
      #region Fields



      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor



      #endregion

      #region Public Methods

      public static void InsertNodeInList(IDomainObjectTreeViewNode domainObjectTreeViewNode, ObservableCollection<IDomainObjectTreeViewNode> nodes)
      {
         if (domainObjectTreeViewNode.DomainObjectViewModel.DomainObject is IIndexable indexableDomainObject)
         {
            if (!nodes.Any())
            {
               nodes.Add(domainObjectTreeViewNode);
            }
            else
            {
               var previousItems = nodes.Where(c => ((IIndexable)c.DomainObjectViewModel.DomainObject).Index <= indexableDomainObject.Index);

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

      #endregion

      #region Private Methods



      #endregion
   }
}
