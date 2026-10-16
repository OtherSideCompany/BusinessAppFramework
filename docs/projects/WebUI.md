# BusinessAppFramework.WebUI

**Layer:** presentation. **Depends on:** Adapter, Application, Domain, Contracts. **Used by:** the host application and the modules' Blazor components.

## Role

WebUI is a Blazor component library built on MudBlazor. It renders the descriptors, actions and workflows defined in the Application layer and talks to the REST API exposed by Adapter. It contains no page of its own: the host and the modules compose the application from these components.

## What's inside

**Components**

- `Pages`: `WorkspaceHost` and `WorkspaceView` render the component named by a workspace descriptor. `DomainObjectPages` provide the default detail page of a domain object with its header, tabs, history, changes popover and chain breadcrumb. `SettingsPage` and `DocumentGeneratorViewerPage` cover settings and document preview.
- `Browser`: `DomainObjectBrowser`, the searchable list driven by a `DomainObjectBrowserDescriptor`, with constraint chips and action buttons.
- `Editor`: property editors for a domain object, including reference and reference list fields, inline properties and categories.
- `Selector` and `Dialog`: pickers and dialogs to choose, create or edit domain objects, confirm or display errors.
- `SearchList`: templates, pagination and value indicators for search results.
- `Kpi`, `Workflow`, `Images`, `Mail`, `NavigationMenu`, `General`: cards, workflow display, image upload and viewer, mail dialog, menu, data grids and badges.

**Gateways**

- `HttpService` and the gateways built on it (`DomainObjectServiceGateway`, `DomainObjectSearchGateway`, `WorkflowServiceGateway`, `TreeGateway`, `RelationServiceGateway`, `DocumentGeneratorGateway`, settings and authentication gateways). Each wraps the calls to one controller of the Adapter, using the routes defined in Contracts.

**Services and registries**

- `ApplicationActionExecutionService`: runs an application action from a click, whether it is an HTTP call, a navigation, a download or a dialog.
- `WorkspaceNavigationService`, `UserDialogService`, `DomainObjectCreationDialogService`, `LocalStorageService`.
- `Authentication`: cookie-based authentication state and a handler that attaches the JWT to API calls.
- `ComponentRegistry`, `DomainObjectBrowserWorkspaceRegistry`, `RelationSelectorRegistry`, `IconFactory`: where modules register their components, workspaces, selectors and icons by key.

## In practice

A module gets a working browser, detail page and editor for a new domain object type by registering a descriptor, a search list template and an icon. It writes custom Razor components only for screens that go beyond the generic ones.
