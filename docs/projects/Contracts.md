# BusinessAppFramework.Contracts

**Layer:** shared, innermost. **Depends on:** nothing. **Used by:** every other project.

## Role

Contracts holds the constants that the backend and the frontend must agree on. It has no behaviour and no business type, so it can be referenced from anywhere, including the Blazor client, without dragging in the domain model.

## What's inside

- **Keys**: `ActionKeys`, `ConstraintKeys`, `DomainErrorKeys`, `MessageKeys`, `SettingsKeys`, `StringKeys`, `EnumKeys`. String identifiers for actions, filters, errors, translated strings and settings.
- **Aggregate keys**: `DomainObjectAggregateKeys` and `DomainObjectSearchResultAggregateKeys` derive a stable route key from a domain object type.
- **API routes**: the `ApiRoutes` folder defines the URL segments of the REST API (domain objects, search, trees, workflows, documents, navigation, relationships) and the `ApiRoute` helper that assembles them.
- **Enums**: `Language` and `TimeUnit`, with extension methods.
- `KebabStringFormatter`: turns type names into kebab-case route segments.

## Why it is a separate project

Putting these constants in Domain would force the frontend to reference business types just to build a URL. Keeping them apart also makes the HTTP contract between Adapter and WebUI explicit and easy to find.
