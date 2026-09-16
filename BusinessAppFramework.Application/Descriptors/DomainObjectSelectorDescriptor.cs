using BusinessAppFramework.Application.Search;
using BusinessAppFramework.Domain.DomainObjects;

namespace BusinessAppFramework.Application.Descriptors
{
   public class DomainObjectSelectorDescriptor<TDomainObject, TSearchResult> : WorkspaceDescriptor, IDomainObjectSelectorDescriptor
       where TDomainObject : DomainObject, new()
       where TSearchResult : DomainObjectSearchResult, new()
   {
      public Type DomainObjectType => typeof(TDomainObject);
      public Type SearchResultType => typeof(TSearchResult);
      public Type SearchListTemplateProviderType { get; init; } = default!;
      public List<string> ConstraintKeys { get; init; } = new();
      public string DefaultConstraintKey { get; set; } = default!;

      public DomainObjectSelectorDescriptor(List<string>? constraintKeys = null)
      {
         ConstraintKeys = new List<string>()
         {
            Contracts.ConstraintKeys.AllConstraintKey
         };

         DefaultConstraintKey = Contracts.ConstraintKeys.AllConstraintKey;

         if (constraintKeys != null)
         {
            ConstraintKeys.AddRange(constraintKeys);
         }
      }
   }
}
