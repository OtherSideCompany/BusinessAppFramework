using BusinessAppFramework.Application.Actions;
using BusinessAppFramework.Application.Factories;
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
        public List<IApplicationAction> DomainObjectApplicationActions { get; init; } = new();
        public List<string> ConstraintKeys { get; init; } = new();
        public string DefaultConstraintKey { get; init; }
        public string PageNavigationApplicationActionKey { get; set; }

        public DomainObjectBrowserDescriptor(
            IDomainObjectRouteKeyRegistry domainObjectRouteKeyRegistry,
            IDomainObjectNavigationApplicationActionFactory domainObjectNavigationApplicationActionFactory,
            string pageNavigationApplicationActionKey,
            List<string>? constraintKeys = null)
        {
            PageNavigationApplicationActionKey = pageNavigationApplicationActionKey;

            ApplicationActions = new List<IApplicationAction>();
            DomainObjectApplicationActions = new List<IApplicationAction>();

            var createAction = new DomainObjectHttpApplicationAction
            {
                ActionKey = ActionKeys.CreateActionKey,
                HttpMethod = HttpMethod.Post,
            };

            createAction.ExecuteRoute =
                $"{ApiRouteSegments.Root}/" +
                $"{ApiRouteSegments.DomainObjects}/" +
                $"{domainObjectRouteKeyRegistry.GetRouteKey<TDomainObject>()}/" +
                $"{ApiRouteSegments.Create}";

            var importExportAction = new DomainObjectHttpApplicationAction
            {
                ActionKey = ActionKeys.ImportExportDataActionKey,
                HttpMethod = HttpMethod.Post,
            };

            ApplicationActions.Add(createAction);
            ApplicationActions.Add(importExportAction);

            var deleteAction = new DomainObjectHttpApplicationAction
            {
                ActionKey = ActionKeys.DeleteActionKey,
                HttpMethod = HttpMethod.Delete
            };

            deleteAction.ExecuteRoute =
                $"{ApiRouteSegments.Root}/" +
                $"{ApiRouteSegments.DomainObjects}/" +
                $"{domainObjectRouteKeyRegistry.GetRouteKey<TDomainObject>()}/" +
                $"{ApiRouteSegments.Delete}/" +
                $"{ApiRouteParams.DomainObjectId}";

            var pageNavigationAction = domainObjectNavigationApplicationActionFactory.Get(StringKey.From(PageNavigationApplicationActionKey));

            DomainObjectApplicationActions.Add(pageNavigationAction);
            DomainObjectApplicationActions.Add(deleteAction);

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

        public void RemoveDefaultApplicationAction(string actionKey)
        {
            var action = ApplicationActions.Where(aa => aa.ActionKey.Equals(actionKey)).FirstOrDefault();

            if (action != null)
            {
                ApplicationActions.Remove(action);
            }
        }

        public void RemoveDefaultDomainObjectApplicationAction(string actionKey)
        {
            var action = DomainObjectApplicationActions.Where(aa => aa.ActionKey.Equals(actionKey)).FirstOrDefault();

            if (action != null)
            {
                DomainObjectApplicationActions.Remove(action);
            }
        }
    }
}
