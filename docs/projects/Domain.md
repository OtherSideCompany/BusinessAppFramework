# BusinessAppFramework.Domain

**Layer:** domain. **Depends on:** Contracts. **Used by:** Application, Infrastructure, Adapter, DocumentRendering, WebUI.

## Role

Domain defines what a business object *is* in the framework. It contains no persistence, no HTTP and no UI code. Every business module derives its own objects from the types found here.

## What's inside

- `DomainObject`: the abstract base class of every business object. It carries an `Id` and audit fields (creation and last modification date, user id and user name), all tagged with `SystemPropertyAttribute` so the UI can tell them apart from business properties. It also knows how to clone itself and how to enumerate its references.
- `DomainObjectReference` and `DomainObjectReferenceList`: typed links from one domain object to one or many others, identified by a relation key. They hold the target id plus a display value so the UI can show them without an extra round trip.
- `HistoryInfo`: the audit information as a value object, shared with persistence entities.
- `LocalizedText`: a dictionary of culture to text with fallback resolution, plus its JSON converter.
- Marker interfaces: `IIndexable` for objects that have a user-defined order, `ISystemObject` for objects identified by a system code and protected from deletion, `IHasReferenceNumber` for objects that carry a human-readable number.
- `IPasswordService`: the only service contract in Domain, used for password hashing.
- `UserRolePermissionType` and `GlobalVariables`.

## In practice

A module creates a class such as `Customer : DomainObject`, adds business properties and, for links to other objects, properties of type `DomainObjectReference`. The rest of the framework (services, repositories, search, UI) works on any such class.
