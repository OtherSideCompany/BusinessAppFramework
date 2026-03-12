using BusinessAppFramework.Application.Actions;
using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Search;
using BusinessAppFramework.Contracts;
using BusinessAppFramework.Contracts.ApiRoutes;
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
        public List<IApplicationAction> ApplicationActions { get; init; } = new();
        public List<IDomainObjectApplicationAction> DomainObjectApplicationActions { get; init; } = new();
        public List<string> ConstraintKeys { get; init; } = new();

        public DomainObjectBrowserDescriptor(
            IDomainObjectPageWorkspaceKeyRegistry domainObjectPageWorkspaceKeyResolver,
            IDomainObjectRouteKeyRegistry domainObjectRouteKeyRegistry,
            List<string>? constraintKeys = null)
        {
            ApplicationActions = new List<IApplicationAction>();
            DomainObjectApplicationActions = new List<IDomainObjectApplicationAction>();

            var createAction = new DomainObjectHttpApplicationAction<TDomainObject>
            {
                ActionKey = StringKey.From(ActionKeys.CreateActionKey),
                HttpMethod = HttpMethod.Post,
            };

            createAction.ExecuteRoute =
                $"{ApiRouteSegments.Root}/" +
                $"{ApiRouteSegments.DomainObjects}/" +
                $"{domainObjectRouteKeyRegistry.GetRouteKey<TDomainObject>()}/" +
                $"{ApiRouteSegments.Create}";

            var importExportAction = new DomainObjectHttpApplicationAction<TDomainObject>
            {
                ActionKey = StringKey.From(ActionKeys.ImportExportDataActionKey),
                HttpMethod = HttpMethod.Post,
            };

            ApplicationActions.Add(createAction);
            ApplicationActions.Add(importExportAction);

            var deleteAction = new DomainObjectHttpApplicationAction<TDomainObject>
            {
                ActionKey = StringKey.From(ActionKeys.DeleteActionKey),
                HttpMethod = HttpMethod.Delete
            };

            deleteAction.ExecuteRoute =
                $"{ApiRouteSegments.Root}/" +
                $"{ApiRouteSegments.DomainObjects}/" +
                $"{domainObjectRouteKeyRegistry.GetRouteKey<TDomainObject>()}/" +
                $"{ApiRouteSegments.Delete}/" +
                $"{ApiRouteParams.DomainObjectId}";

            var pageNavigationAction = new DomainObjectNavigationApplicationAction<TDomainObject>(domainObjectPageWorkspaceKeyResolver)
            {
                ActionKey = StringKey.From(ActionKeys.DetailsActionKey)
            };
            
            DomainObjectApplicationActions.Add(pageNavigationAction);
            DomainObjectApplicationActions.Add(deleteAction);

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
