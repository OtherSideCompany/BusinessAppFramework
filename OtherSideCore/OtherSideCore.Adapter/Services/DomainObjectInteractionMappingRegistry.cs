using OtherSideCore.Domain;

namespace OtherSideCore.Adapter.Services
{
   public class DomainObjectInteractionMappingRegistry
   {
      #region Fields

      private readonly List<DomainObjectInteractionMapping> _mappings = new();

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectInteractionMappingRegistry()
      {

      }

      #endregion

      #region Public Methods

      public void Register(DomainObjectInteractionMapping mapping)
      {
         if (TryGetByDomainObjectType(mapping.DomainObjectType, out _))
         {
            throw new ArgumentException($"Cannot add several mapping with domain object type {mapping.DomainObjectType}");
         }

         _mappings.Add(mapping);
      }

      public bool TryGetByDomainObjectType(Type type, out DomainObjectInteractionMapping? mapping)
      {
         mapping = _mappings.FirstOrDefault(m => m.DomainObjectType == type);
         return mapping != null;
      }

      public bool TryGetByDomainObjectViewModelType(Type type, out DomainObjectInteractionMapping? mapping)
      {
         mapping = _mappings.FirstOrDefault(m => m.DomainObjectViewModelType == type);
         return mapping != null;
      }

      public bool TryGetBySearchResultType(Type type, out DomainObjectInteractionMapping? mapping)
      {
         mapping = _mappings.FirstOrDefault(m => m.SearchResultType == type);
         return mapping != null;
      }

      public bool TryGetByTreeNodeViewModelType(Type type, out DomainObjectInteractionMapping? mapping)
      {
         mapping = _mappings.FirstOrDefault(m => m.TreeNodeViewModelType == type);
         return mapping != null;
      }

      public bool TryGetBySelectorKey(StringKey key, out DomainObjectInteractionMapping? mapping)
      {
         mapping = _mappings.FirstOrDefault(m => m.SelectorKey == key);
         return mapping != null;
      }

      public bool TryGetByWorkspaceKey(StringKey key, out DomainObjectInteractionMapping? mapping)
      {
         mapping = _mappings.FirstOrDefault(m => m.WorkspaceKey == key);
         return mapping != null;
      }

      public bool TryGetByEditorKey(StringKey key, out DomainObjectInteractionMapping? mapping)
      {
         mapping = _mappings.FirstOrDefault(m => m.EditorKey == key);
         return mapping != null;
      }

      public bool TryGetByDetailsEditorKey(StringKey key, out DomainObjectInteractionMapping? mapping)
      {
         mapping = _mappings.FirstOrDefault(m => m.DetailsEditorKey == key);
         return mapping != null;
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
