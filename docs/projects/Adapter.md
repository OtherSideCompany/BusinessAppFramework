# BusinessAppFramework.Adapter

**Layer:** entry point (server side). **Depends on:** Application, Domain, Contracts. **Used by:** WebUI (for the HTTP contract), the host application.

## Role

Adapter exposes the application layer as a REST API with ASP.NET Core controllers. It is the server-side door through which the Blazor frontend, or any other client, reaches the framework.

## What's inside

- `DomainObjectController<TDomainObject, TSearchResult>`: generic create / read / save / delete endpoints for one domain object type. Business errors are returned as a payload rather than as HTTP errors so the UI can display them.
- `DomainObjectSearchController`: paginated and constrained search for one domain object type.
- `AuthenticationController`: login and token issuance.
- `DocumentGeneratorController`: HTML preview and PDF download of generated documents.
- `WorkflowController`, `TreeController`, `RelationServiceController`, `DomainObjectChainController`, `NavigationController`: expose the corresponding application services.
- `ModuleSettingsController` and `UserSettingsController`.
- `IWebApiModuleBootstrapper`: the contract a module implements to declare its closed generic controllers and their route keys.
- `Requests` and `Responses`: the few DTOs that are specific to the HTTP layer (login request, authentication response, document responses).

## In practice

A module does not write controllers. It lists the closed generic controller types it needs, for example `DomainObjectController<Customer, CustomerSearchResult>`, and the Bootstrapper project registers them with ASP.NET Core at startup. Routes are built from the segments defined in Contracts.
