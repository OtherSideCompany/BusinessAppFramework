using BusinessAppFramework.Application.ActionResult;
using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Search;
using BusinessAppFramework.Contracts;
using BusinessAppFramework.Domain;
using BusinessAppFramework.Domain.DomainObjects;

namespace BusinessAppFramework.Application.Descriptors
{
   public class DomainObjectBrowserDescriptor<TDomainObject, TSearchResult> : WorkspaceDescriptor
       where TDomainObject : DomainObject, new()
       where TSearchResult : DomainObjectSearchResult, new()
   {
      public Type DomainObjectType => typeof(TDomainObject);
      public Type SearchResultType => typeof(TSearchResult);
      public Type SearchListTemplateProviderType { get; init; } = default!;
      public Type? DetailEditorComponentType { get; init; } = default;
      public List<IDomainObjectApplicationAction> Actions { get; init; } = new();
      public List<string> ConstraintKeys { get; init; } = new();

      public DomainObjectBrowserDescriptor(
          IDomainObjectPageWorkspaceKeyRegistry domainObjectPageWorkspaceKeyResolver,
          List<string>? constraintKeys = null)
      {
         Actions = new List<IDomainObjectApplicationAction>
            {
                new DomainObjectHttpApplicationAction<TDomainObject>
                {
                    ActionKey = StringKey.From(ActionKeys.CreateActionKey),
                    ExecuteRouteTemplate = Routes.CreateTemplate,
                    HttpMethod = HttpMethod.Post
                },
                new DomainObjectHttpApplicationAction<TDomainObject>
                {
                    ActionKey = StringKey.From(ActionKeys.DeleteActionKey),
                    ExecuteRouteTemplate = Routes.DeleteTemplate,
                    HttpMethod = HttpMethod.Delete,
                    RequireDomainObjectId = true
                },
                new DomainObjectNavigationApplicationAction<TDomainObject>(domainObjectPageWorkspaceKeyResolver)
                {
                    ActionKey = StringKey.From(ActionKeys.DetailsActionKey),
                    RequireDomainObjectId = true
                }
            };
         ConstraintKeys = new List<string>()
            {
                Contracts.ConstraintKeys.AllConstraintKey
            };

         if (constraintKeys != null)
         {
            ConstraintKeys.AddRange(constraintKeys);
         }
      }
   }
}
