# BusinessAppFramework.Bootstrapper

**Layer:** entry point (composition). **Depends on:** Application. **Used by:** the host application.

## Role

Bootstrapper is a small project of helpers that a host uses to assemble the framework and its modules at startup. It deals with the one thing ASP.NET Core does not do by itself: discovering and routing closed generic controllers.

## What's inside

- `ClosedGenericControllerFeatureProvider`: adds the closed generic controller types declared by the modules (for example `DomainObjectController<Customer, CustomerSearchResult>`) to the MVC controller feature.
- `ClosedGenericRouteConvention`: gives each of those controllers its route from the module's route key.
- `ServiceRegistrationHelper`: registers an implementation together with every generic interface it implements for a given domain object type, in one call.
- `IBootstrapService` and `IBootstrapConfiguration`: the contract the host implements to provide the service collection and basic configuration such as the log file name.

## In practice

The host's startup code collects the `IModuleBootstrapper` and `IWebApiModuleBootstrapper` instances of every module, feeds the controller descriptions to the feature provider and the convention, and uses the registration helper to wire services, repositories and search services per domain object type.
