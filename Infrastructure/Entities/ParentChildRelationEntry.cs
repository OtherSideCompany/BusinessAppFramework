using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;

namespace Infrastructure.Entities
{
   public class ParentChildRelationEntry<TParentEntity, TChildEntity> : IInfrastructureParentChildRelation
       where TChildEntity : IEntity
       where TParentEntity : class, IEntity
   {
      private StringKey _relationKey;
      private readonly Func<DbContext, int, IQueryable<int>> _childrenIdsGetter;

      public StringKey RelationKey => _relationKey;
      public Type ChildEntityType => typeof(TChildEntity);
      public Type ParentEntityType => typeof(TParentEntity);
      public PropertyInfo ParentEntityIdProperty { get; }

      public ParentChildRelationEntry(
         StringKey key,
         PropertyInfo parentEntityIdProperty,
         Func<DbContext, int, IQueryable<int>> childrenIdsGetter)
      {
         _relationKey = key;
         _childrenIdsGetter = childrenIdsGetter;

         ParentEntityIdProperty = parentEntityIdProperty;
      }

      public IQueryable<int> GetChildrenIds(DbContext context, int parentId)
      {
         return _childrenIdsGetter(context, parentId);
      }
   }
}
