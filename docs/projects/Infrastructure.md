# BusinessAppFramework.Infrastructure

**Layer:** infrastructure. **Depends on:** Application, Domain, Contracts. **Used by:** the host application.

## Role

Infrastructure implements the ports declared in Application with concrete technologies. It is the only framework project that knows about Entity Framework Core, Serilog, AutoMapper, Magick.NET or JWT.

## What's inside

**Persistence**

- `OtherSideCoreDbContext`: the abstract EF Core context every host derives from. It restricts cascade deletes on all foreign keys and registers an `EditDistance` database function used by fuzzy search.
- `SqlServerSSPIDbContext` and `SqliteDbContext`: ready-made contexts for the two supported providers.
- `IModelBuilderContributor`: lets modules add their entity configuration to the model. The framework contributes module settings and user settings itself.
- `ISeedContributor`, `DbInitializerService`, `WebDbInitializerService`: database creation and seeding at startup.
- `Repositories`: `Repository<TDomainObject, TEntity>` maps domain objects to persistence entities with AutoMapper and performs CRUD. `IndexableRepository`, `StatusRepository`, `ModuleSettingsRepository` and `UserSettingsRepository` specialise it.
- `Entities`: the framework's own entities (module settings, user settings) and the relation entries used to resolve references and parent/child links.

**Search and relations**

- `SearchService<TSearchResult>`: paginated, sorted and constrained search projected directly from the database.
- `RelationService`, `ReferenceResolver`, `ParentChildRelationResolver`, `DomainObjectTypeMap`: resolve typed links between domain objects and build trees.

**Technical services**

- `JwtAuthenticationTokenService` and `PasswordService` for authentication.
- `Logger` built on Serilog with a file sink.
- `MailtoMailService`, `PrintersService`, `FileSystemDocumentStorageService`, `JsonFileSerializationService`, `ImageCompressionService`.
- `WorkflowContextLoader`: loads the data a workflow needs to evaluate its state.

## In practice

A host picks a database context, registers the repositories and services from this project, and adds the module's model builder contributors and seed contributors. Business modules usually do not write infrastructure code beyond entity classes and mapping profiles.
