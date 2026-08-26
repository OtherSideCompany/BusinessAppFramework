using BusinessAppFramework.Application.Actions;
using BusinessAppFramework.Application.Factories;
using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Contracts;
using BusinessAppFramework.Contracts.ApiRoutes;

namespace BusinessAppFramework.Application.Descriptors
{
    public class DomainObjectBrowserDescriptor : WorkspaceDescriptor
    {
        private DomainObjectHttpApplicationAction _createAction;
        private DomainObjectHttpApplicationAction _deleteAction;

        public Type DomainObjectType { get; private set; } = default!;
        public Type SearchListTemplateProviderType { get; set; } = default!;
        public List<IApplicationAction> ApplicationActions { get; init; } = new();
        public List<IApplicationAction> DomainObjectApplicationActions { get; init; } = new();
        public List<string> ConstraintKeys { get; init; } = new();
        public string DefaultConstraintKey { get; set; } = default!;
        public string PageNavigationApplicationActionKey { get; set; } = default!;

        public DomainObjectBrowserDescriptor(
            Type domainObjectType,
            IDomainObjectNavigationApplicationActionFactory domainObjectNavigationApplicationActionFactory,
            string pageNavigationApplicationActionKey,
            List<string>? constraintKeys = null)
        {
            DomainObjectType = domainObjectType;
            PageNavigationApplicationActionKey = pageNavigationApplicationActionKey;

            ApplicationActions = new List<IApplicationAction>();
            DomainObjectApplicationActions = new List<IApplicationAction>();

            _createAction = new DomainObjectHttpApplicationAction
            {
                ActionKey = ActionKeys.CreateActionKey,
                HttpMethod = HttpMethod.Post,
            };

            var importExportAction = new DomainObjectHttpApplicationAction
            {
                ActionKey = ActionKeys.ImportExportDataActionKey,
                HttpMethod = HttpMethod.Post,
            };

            ApplicationActions.Add(_createAction);
            ApplicationActions.Add(importExportAction);

            _deleteAction = new DomainObjectHttpApplicationAction
            {
                ActionKey = ActionKeys.DeleteActionKey,
                HttpMethod = HttpMethod.Delete
            };

            SetActionRoutes();

            if (!String.IsNullOrEmpty(PageNavigationApplicationActionKey))
            {
                var pageNavigationAction = domainObjectNavigationApplicationActionFactory.Get(PageNavigationApplicationActionKey);
                DomainObjectApplicationActions.Add(pageNavigationAction);
            }

            DomainObjectApplicationActions.Add(_deleteAction);

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

        public void SetDomainObjectType(Type domainObjectType)
        {
            DomainObjectType = domainObjectType;
            SetActionRoutes();
        }

        private void SetActionRoutes()
        {
            _createAction.ExecuteRoute = $"{ApiRoute.DomainObjectControllerRoute(DomainObjectType)}/{ApiRouteSegments.Create}";
            _deleteAction.ExecuteRoute = $"{ApiRoute.DomainObjectControllerRoute(DomainObjectType)}/{ApiRouteSegments.Delete}/{ApiRouteParams.DomainObjectId}";
        }
    }
}
