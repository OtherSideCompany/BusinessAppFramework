using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Domain;
using BusinessAppFramework.Domain.DomainObjects;
using System.Linq.Expressions;

namespace BusinessAppFramework.Application.Relations
{
    public interface IReferenceResolver
    {
        bool TryGetReferenceRelationEntry(string key, out IReferenceRelationEntry relationEntry);
        bool TryGetReferenceListRelationEntry(string key, out IReferenceListRelationEntry relationEntry);
        IEnumerable<IReferenceRelationEntry> GetReferenceRelationEntriesBySourceType(Type sourceType);
        IEnumerable<IReferenceListRelationEntry> GetReferenceListRelationEntriesBySourceType(Type sourceType);
        public void RegisterReferenceRelationEntry<TSourceDomainObject, TTargetDomainObject, TSourceEntity, TTargetEntity>(
           Expression<Func<TSourceDomainObject, DomainObjectReference>> domainExpression,
           Expression<Func<TSourceEntity, int?>> entityIdExpression)
            where TSourceDomainObject : DomainObject
            where TTargetDomainObject : DomainObject
            where TSourceEntity : class, IEntity
            where TTargetEntity : class, IEntity;

        public void RegisterReferenceRelationEntry<TSourceDomainObject, TTargetDomainObject, TSourceEntity, TTargetEntity>(
           string relationKey,
           Expression<Func<TSourceDomainObject, DomainObjectReference>> domainExpression,
           Expression<Func<TSourceEntity, int?>> entityIdExpression)
            where TSourceDomainObject : DomainObject
            where TTargetDomainObject : DomainObject
            where TSourceEntity : class, IEntity
            where TTargetEntity : class, IEntity;

        public void RegisterReferenceListRelationEntry<TSourceDomainObject, TTargetDomainObject, TSourceEntity, TTargetEntity>(
              Expression<Func<TSourceDomainObject, DomainObjectReferenceList>> domainExpression,
              Expression<Func<TSourceEntity, ICollection<TTargetEntity>>> entityExpression)
               where TSourceDomainObject : DomainObject
               where TTargetDomainObject : DomainObject
               where TSourceEntity : class, IEntity
               where TTargetEntity : class, IEntity;

        public void RegisterReferenceListRelationEntry<TSourceDomainObject, TTargetDomainObject, TSourceEntity, TTargetEntity>(
              string relationKey,
              Expression<Func<TSourceDomainObject, DomainObjectReferenceList>> domainExpression,
              Expression<Func<TSourceEntity, ICollection<TTargetEntity>>> entityExpression)
               where TSourceDomainObject : DomainObject
               where TTargetDomainObject : DomainObject
               where TSourceEntity : class, IEntity
               where TTargetEntity : class, IEntity;        
    }
}
