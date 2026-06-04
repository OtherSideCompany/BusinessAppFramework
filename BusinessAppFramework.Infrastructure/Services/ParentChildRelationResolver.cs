using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Relations;
using BusinessAppFramework.Domain;
using BusinessAppFramework.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BusinessAppFramework.Infrastructure.Services
{
    public class ParentChildRelationResolver : IParentChildRelationRegistry, IParentChildRelationResolver
    {
        #region Fields

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

        

        public bool TryGetParentChildRelationEntry(string key, out IParentChildRelationEntry relationEntry)
        {
            relationEntry = _parentChildRelationEntries.FirstOrDefault(r => r.RelationKey.Equals(key));
            return relationEntry != null;
        }

       

        public void RegisterParentChildRelationEntry<TParentEntity, TChildEntity>(
              string relationKey,
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

        #region Private Methods

        private static PropertyInfo ExtractProperty<TSourceEntity>(Expression<Func<TSourceEntity, int?>> expr)
        {
            if (expr.Body is MemberExpression member)
                return (PropertyInfo)member.Member;

            if (expr.Body is UnaryExpression unary && unary.Operand is MemberExpression unaryMember)
                return (PropertyInfo)unaryMember.Member;

            throw new InvalidOperationException("Invalid reference property expression.");
        }

        #endregion
    }
}
