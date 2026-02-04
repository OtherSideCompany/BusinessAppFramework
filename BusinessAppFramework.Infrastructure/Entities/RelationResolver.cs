using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Relations;
using BusinessAppFramework.Domain;
using BusinessAppFramework.Domain.DomainObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BusinessAppFramework.Infrastructure.Entities
{
   public abstract class RelationResolver : IRelationResolver
   {
      #region Fields

      private readonly List<IReferenceRelationEntry> _referenceRelationEntries = new();
      private readonly List<IReferenceListRelationEntry> _referenceListRelationEntries = new();
      private readonly List<IParentChildRelationEntry> _parentChildRelationEntries = new();

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor



      #endregion

      #region Public Methods

      public bool ContainsParentChildRelationByChildType(Type childType, Type parentType)
      {
         return _parentChildRelationEntries.Any(r => r.ChildEntityType == childType && r.ParentEntityType == parentType);
      }

      public bool TryGetReferenceRelationEntry(StringKey key, out IReferenceRelationEntry relationEntry)
      {
         relationEntry = _referenceRelationEntries.FirstOrDefault(r => r.RelationKey.Equals(key));
         return relationEntry != null;
      }

      public bool TryGetReferenceListRelationEntry(StringKey key, out IReferenceListRelationEntry relationEntry)
      {
         relationEntry = _referenceListRelationEntries.FirstOrDefault(r => r.RelationKey.Equals(key));
         return relationEntry != null;
      }

      public bool TryGetParentChildRelationEntry(StringKey key, out IParentChildRelationEntry relationEntry)
      {
         relationEntry = _parentChildRelationEntries.FirstOrDefault(r => r.RelationKey.Equals(key));
         return relationEntry != null;
      }

      public IEnumerable<IReferenceRelationEntry> GetReferenceRelationEntriesBySourceType(Type sourceType)
      {
         for (var t = sourceType; t != null && t != typeof(object); t = t.BaseType)
         {
            foreach (var entry in _referenceRelationEntries.Where(r => r.SourceEntityType == t))
               yield return entry;
         }
      }

      public IEnumerable<IReferenceListRelationEntry> GetReferenceListRelationEntriesBySourceType(Type sourceType)
      {
         for (var t = sourceType; t != null && t != typeof(object); t = t.BaseType)
         {
            foreach (var entry in _referenceListRelationEntries.Where(r => r.SourceEntityType == t))
               yield return entry;
         }
      }

      #endregion

      #region Private Methods

      private static PropertyInfo ExtractProperty<TSourceDomainObject>(Expression<Func<TSourceDomainObject, DomainObjectReference>> expr)
      {
         if (expr.Body is MemberExpression member)
            return (PropertyInfo)member.Member;

         if (expr.Body is UnaryExpression unary && unary.Operand is MemberExpression unaryMember)
            return (PropertyInfo)unaryMember.Member;

         throw new InvalidOperationException("Invalid reference property expression.");
      }

      private static PropertyInfo ExtractProperty<TSourceDomainObject>(Expression<Func<TSourceDomainObject, DomainObjectReferenceList>> expr)
      {
         if (expr.Body is MemberExpression member)
            return (PropertyInfo)member.Member;

         if (expr.Body is UnaryExpression unary && unary.Operand is MemberExpression unaryMember)
            return (PropertyInfo)unaryMember.Member;

         throw new InvalidOperationException("Invalid reference property expression.");
      }

      private static PropertyInfo ExtractProperty<TSourceEntity, TTargetEntity>(Expression<Func<TSourceEntity, ICollection<TTargetEntity>>> expr)
      {
         if (expr.Body is MemberExpression member)
            return (PropertyInfo)member.Member;

         if (expr.Body is UnaryExpression unary && unary.Operand is MemberExpression unaryMember)
            return (PropertyInfo)unaryMember.Member;

         throw new InvalidOperationException("Invalid reference property expression.");
      }

      private static PropertyInfo ExtractProperty<TSourceEntity>(Expression<Func<TSourceEntity, int?>> expr)
      {
         if (expr.Body is MemberExpression member)
            return (PropertyInfo)member.Member;

         if (expr.Body is UnaryExpression unary && unary.Operand is MemberExpression unaryMember)
            return (PropertyInfo)unaryMember.Member;

         throw new InvalidOperationException("Invalid reference property expression.");
      }

      protected void RegisterReferenceRelationEntry<TSourceDomainObject, TTargetDomainObject, TSourceEntity, TTargetEntity>(
         StringKey relationKey,
         Expression<Func<TSourceDomainObject, DomainObjectReference>> domainExpression,
         Expression<Func<TSourceEntity, int?>> entityIdExpression)
          where TSourceDomainObject : DomainObject
          where TTargetDomainObject : DomainObject
          where TSourceEntity : IEntity
          where TTargetEntity : class, IEntity
      {
         var domainProperty = ExtractProperty(domainExpression);
         var entityIdProperty = ExtractProperty(entityIdExpression);

         var entry = new ReferenceRelationEntry<TSourceDomainObject, TTargetDomainObject, TSourceEntity, TTargetEntity>(relationKey, domainProperty, entityIdProperty);

         if (_referenceRelationEntries.Any(r => r.RelationKey.Equals(relationKey)))
         {
            throw new ArgumentException($"Cannot add several reference relation entries with key {relationKey}");
         }

         _referenceRelationEntries.Add(entry);
      }

      protected void RegisterReferenceListRelationEntry<TSourceDomainObject, TTargetDomainObject, TSourceEntity, TTargetEntity>(
            StringKey relationKey,
            Expression<Func<TSourceDomainObject, DomainObjectReferenceList>> domainExpression,
            Expression<Func<TSourceEntity, ICollection<TTargetEntity>>> entityExpression)
             where TSourceDomainObject : DomainObject
             where TTargetDomainObject : DomainObject
             where TSourceEntity : IEntity
             where TTargetEntity : class, IEntity
      {
         var domainProperty = ExtractProperty(domainExpression);
         var entityProperty = ExtractProperty(entityExpression);

         var entry = new ReferenceListRelationEntry<TSourceDomainObject, TTargetDomainObject, TSourceEntity, TTargetEntity>(relationKey, domainProperty, entityProperty);

         if (_referenceListRelationEntries.Any(r => r.RelationKey.Equals(relationKey)))
         {
            throw new ArgumentException($"Cannot add several reference list relation entries with key {relationKey}");
         }

         _referenceListRelationEntries.Add(entry);
      }

      protected void RegisterParentChildRelationEntry<TParentEntity, TChildEntity>(
            StringKey relationKey,
            Expression<Func<TChildEntity, int?>> parentEntityIdExpression,
            Func<DbContext, int, IQueryable<int>> childrenIdsGetter)
             where TChildEntity : class, IEntity
             where TParentEntity : class, IEntity
      {
         var parentEntityIdProperty = ExtractProperty(parentEntityIdExpression);
         var entry = new ParentChildRelationEntry<TParentEntity, TChildEntity>(relationKey, parentEntityIdProperty, childrenIdsGetter);

         if (_parentChildRelationEntries.Any(r => r.RelationKey.Equals(relationKey)))
         {
            throw new ArgumentException($"Cannot add several relation entries with key {relationKey}");
         }
         else if (ContainsParentChildRelationByChildType(typeof(TChildEntity), typeof(TParentEntity)))
         {
            throw new ArgumentException($"Cannot add several parent child relations entries for types <{typeof(TChildEntity)},{typeof(TParentEntity)}>");
         }

         _parentChildRelationEntries.Add(entry);
      }

      #endregion
   }
}
