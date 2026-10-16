# BusinessAppFramework.Application

**Layer:** application. **Depends on:** Domain, Contracts. **Used by:** Infrastructure, Adapter, DocumentRendering, Bootstrapper, WebUI.

## Role

Application is the largest project and the center of the framework. It defines the use cases, the ports that the infrastructure implements, and the declarative building blocks that the UI renders. It is shared by the server and the Blazor client.

## What's inside

**Use cases and services**

- `IDomainObjectService<T>` / `DomainObjectService<T>`: create, read, save, delete and validate any domain object, hydrate its references and publish events.
- `WorkflowService`, `AuthenticationService`, `CalendarService`, `LocalizedStringService`, `ModuleSettingsService`, `UserSettingsService`, `ReferenceHydrator`.

**Ports (interfaces)**

- Persistence and search: `IRepository<T>`, `IIndexableRepository`, `IStatusRepository`, `ISearchService<T>`, `ISearchGateway`.
- Relations: `IRelationService`, `IReferenceResolver`, `IParentChildRelationResolver`, `IDomainObjectTypeMap`.
- Technical services: `IFileSystemService`, `IFileSerializationService`, `IDocumentStorageService`, `IImageCompressionService`, `IMailService`, `IPrintersService`, `IAuthenticationTokenService`, `ICurrentUserService`, `IUserDialogService`.

**Declarative building blocks**

- `Descriptors`: `WorkspaceDescriptor` names the component that renders a screen; `DomainObjectBrowserDescriptor` describes a searchable list of one domain object type, with its actions and constraints; `DomainObjectSelectorDescriptor` describes a picker.
- `Actions`: `ApplicationAction` and its variants (HTTP, navigation, file download, document generation, open dialog). The UI shows them as buttons, the backend executes them.
- `Browser/Constraint`: a named filter expressed as a LINQ expression over a search result.
- `Workflows`: `ProcessWorkflow`, `Step`, `Transition`, `Condition`, `WorkflowContext`.
- `Trees`: `Tree`, `Branch`, `Node`, `NodeSummary` for parent/child hierarchies.
- `DomainObjectChains`: the navigation chain from one object to its related objects, shown as a breadcrumb.
- `Kpis` and `Activities`: indicators and counters shown on dashboards.
- `Search`: `SearchRequest`, `PaginatedSearchRequest`, `SearchResult`, `DomainObjectSearchResult`.

**Extension points**

- `IModuleBootstrapper`: the contract a business module implements to register everything it contributes.
- `Registry` and the registries built on it (settings, reference navigation keys), plus the factories (`DomainObjectServiceFactory`, `WorkflowFactory`, `TreeFactory`, `ConstraintFactory`, `WorkspaceDescriptorFactory` and others) that resolve registered items by key or by type.
- `DomainObjectEvents`: an in-process event bus with created, saved, deleting and deleted events a module can handle.
- `Mapping/IMappingProfileContributor`: how a module provides its AutoMapper profiles.

## In practice

Most of a module's backend code is configuration against this project: register domain object types, declare descriptors and constraints, write a few event handlers or a workflow. The generic services do the rest.
