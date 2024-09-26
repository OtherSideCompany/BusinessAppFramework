using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using OtherSideCore.Infrastructure;
using OtherSideCore.Infrastructure.DatabaseFields;
using OtherSideCore.Infrastructure.Entities;
using OtherSideCore.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OtherSideCore.Infrastructure.Repositories
{
   public class MockDataRepository<T> : IDataRepository<T> where T : EntityBase, new()
   {
      #region Fields

      protected List<T> _entities;

      #endregion

      #region Contructor

      public MockDataRepository()
      {
         _entities = new List<T>();
      }

      #endregion

      #region Public Methods

      public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken)
      {
         return _entities;
      }

      public virtual async Task<List<T>> GetAllAsync(List<string> filters, List<Constraint<T>> constraints, bool extendedSearch, CancellationToken cancellationToken)
      {
         var query = _entities.AsQueryable();

         if (constraints != null)
         {
            foreach (var constraint in constraints)
            {
               query = query.Where(constraint.LambdaConstructor());
            }
         }

         return query.Distinct().ToList();
      }

      public async Task<int> CreateAsync(List<DatabaseField> databaseFields)
      {
         var entityBase = new T();

         entityBase.SetProperties(databaseFields);
         entityBase.Id = _entities.Any() ? _entities.Max(e => e.Id) + 1 : 1;

         _entities.Add(entityBase);

         return entityBase.Id;
      }

      public async Task SaveAsync(int entityId, List<DatabaseField> databaseFields)
      {
         T entity = _entities.FirstOrDefault(e => e.Id == entityId);

         if (entity != null)
         {
            entity.SetProperties(databaseFields);
         }
         else
         {
            throw new ArgumentNullException($"Entity with Id {entityId} not found in data repository {nameof(T).ToString()}");
         }
      }

      public async Task<EntityBase> GetAsync(int entityId, CancellationToken cancellationToken)
      {
         return _entities.FirstOrDefault(e => e.Id == entityId);
      }

      public async Task DeleteAsync(int entityId)
      {
         T entity = _entities.FirstOrDefault(e => e.Id == entityId);

         if (entity != null)
         {
            _entities.Remove(entity);
         }
         else
         {
            throw new ArgumentNullException($"Entity with Id {entityId} not found in data repository {nameof(T).ToString()}");
         }
      }

      public async Task<DateTime> GetModificatonTimeAsync(int entityId)
      {
         T entity = _entities.FirstOrDefault(e => e.Id == entityId);

         if (entity != null)
         {
            return entity.LastModifiedDateTime;
         }
         else
         {
            throw new ArgumentNullException($"Entity with Id {entityId} not found in data repository {nameof(T).ToString()}");
         }
      }

      public void Dispose()
      {
         
      }

      #endregion
   }
}
