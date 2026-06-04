using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace BusinessAppFramework.Infrastructure.Services
{
    public interface IParentChildRelationRegistry
    {        
        void RegisterParentChildRelationEntry<TParentEntity, TChildEntity>(
              string relationKey,
              Expression<Func<TChildEntity, int?>> parentEntityIdExpression,
              Func<DbContext, int, IQueryable<int>> childrenIdsGetter)
               where TChildEntity : class, IEntity
               where TParentEntity : class, IEntity;
    }
}
