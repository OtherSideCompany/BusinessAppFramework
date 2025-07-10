using OtherSideCore.Adapter.DomainObjectInteraction;
using OtherSideCore.Application.Search;
using OtherSideCore.Domain;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Adapter.Services
{
   public class DomainObjectInteractionMapping
   {
      #region Fields



      #endregion

      #region Properties

      public Type DomainObjectType { get; private set; }
      public Type DomainObjectViewModelType { get; private set; }
      public Type? SearchResultType { get; private set; }
      public Type? TreeNodeViewModelType { get; private set; }

      public StringKey? SelectorKey { get; private set; }
      public StringKey? WorkspaceKey { get; private set; }
      public StringKey EditorKey { get; private set; }
      public StringKey? DetailsEditorKey { get; private set; }
      public StringKey? TreeNodeViewModelKey { get; private set; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectInteractionMapping(
         Type domainObjectType,
         Type domainObjectViewModelType,
         Type? searchResultType,         
         Type? treeNodeViewModelType,
         StringKey? selectorKey,
         StringKey? workspaceKey,
         StringKey editorKey,
         StringKey? detailsEditorKey)
      {
         if (!typeof(DomainObject).IsAssignableFrom(domainObjectType))
            throw new ArgumentException($"{domainObjectType.FullName} must inherit from DomainObject", nameof(domainObjectType));

         if (!typeof(DomainObjectViewModel).IsAssignableFrom(domainObjectViewModelType))
            throw new ArgumentException($"{domainObjectViewModelType.FullName} must inherit from DomainObjectViewModel", nameof(domainObjectViewModelType));

         if (searchResultType is not null && !typeof(DomainObjectSearchResult).IsAssignableFrom(searchResultType))
            throw new ArgumentException($"{searchResultType.FullName} must inherit from DomainObjectSearchResult", nameof(searchResultType));

         if (treeNodeViewModelType is not null && !typeof(IDomainObjectTreeNodeViewModel).IsAssignableFrom(treeNodeViewModelType))
            throw new ArgumentException($"{treeNodeViewModelType.FullName} must inherit from IDomainObjectTreeNodeViewModel", nameof(treeNodeViewModelType));

         DomainObjectType = domainObjectType;
         DomainObjectViewModelType = domainObjectViewModelType;
         SearchResultType = searchResultType;                 
         TreeNodeViewModelType = treeNodeViewModelType;
         SelectorKey = selectorKey;
         WorkspaceKey = workspaceKey;
         EditorKey = editorKey;
         DetailsEditorKey = detailsEditorKey;
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
