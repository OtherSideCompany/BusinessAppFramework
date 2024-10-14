using OtherSideCore.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OtherSideCore.Domain.RepositoryInterfaces;
using OtherSideCore.Domain.DomainObjects;
using System.Reflection;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace OtherSideCore.Infrastructure.Repositories
{
   public class MockRepository<T> : IRepository<T> where T : DomainObject, new()
   {
      #region Fields

      protected List<T> _domainObjects;

      #endregion

      #region Contructor

      public MockRepository()
      {
         _domainObjects = new List<T>();
      }

      #endregion

      #region Public Methods

      public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken)
      {
         return _domainObjects;
      }

      public async Task CreateAsync(T domainObject, int userId)
      {
         domainObject.Id = _domainObjects.Any() ? _domainObjects.Max(e => e.Id) + 1 : 1;
         domainObject.CreationDate = DateTime.Now;
         domainObject.LastModifiedDateTime = DateTime.Now;

         if (domainObject.CreatedBy == null)
         {
            domainObject.CreatedBy = new Domain.DomainObjects.User();
         }

         domainObject.CreatedBy.Id = userId;

         if (domainObject.LastModifiedBy == null)
         {
            domainObject.LastModifiedBy = new Domain.DomainObjects.User();
         }
         
         domainObject.LastModifiedBy.Id = userId;

         _domainObjects.Add(domainObject);
      }

      public async Task SaveAsync(T domainObject, int userId)
      {
         T existingEntity = _domainObjects.FirstOrDefault(e => e.Id == domainObject.Id);

         if (existingEntity != null)
         {
            domainObject.LastModifiedDateTime = DateTime.Now;
            domainObject.LastModifiedBy.Id = userId;

            int index = _domainObjects.IndexOf(existingEntity);
            _domainObjects[index] = domainObject;
         }
         else
         {
            throw new ArgumentNullException($"Entity with Id {domainObject.Id} not found in data repository {nameof(T).ToString()}");
         }
      }

      public async Task<T> GetAsync(int domainObjectId, CancellationToken cancellationToken = default)
      {
         return _domainObjects.FirstOrDefault(e => e.Id == domainObjectId);
      }

      public async Task DeleteAsync(T domainObject)
      {
         T existingEntity = _domainObjects.FirstOrDefault(e => e.Id == domainObject.Id);

         if (existingEntity != null)
         {
            _domainObjects.Remove(existingEntity);

            domainObject.Id = 0;
            domainObject.LastModifiedBy = null;
            domainObject.CreatedBy = null;
            domainObject.CreationDate = default;
            domainObject.LastModifiedDateTime = default;
         }
         else
         {
            throw new ArgumentNullException($"Entity with Id {domainObject.Id} not found in data repository {nameof(T).ToString()}");
         }
      }

      public async Task<DateTime> GetLastModificatonTimeAsync(T domainObject)
      {
         T existingDomainObject = _domainObjects.FirstOrDefault(e => e.Id == domainObject.Id);

         if (existingDomainObject != null)
         {
            return existingDomainObject.LastModifiedDateTime;
         }
         else
         {
            throw new ArgumentNullException($"Entity with Id {domainObject.Id} not found in data repository {nameof(T).ToString()}");
         }
      }

      public async Task LoadAsync(T domainObject)
      {
         T existingDomainObject = _domainObjects.FirstOrDefault(e => e.Id == domainObject.Id);

         if (existingDomainObject != null)
         {
            CopyProperties(existingDomainObject, domainObject);
         }
         else
         {
            throw new ArgumentNullException($"Entity with Id {domainObject.Id} not found in data repository {nameof(T).ToString()}");
         }
      }

      public async Task LoadTrackingInfos(T domainObject)
      {
         throw new NotImplementedException();
      }

      public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
      {
         var query = _domainObjects.AsQueryable();

         return await query.Distinct().ToListAsync();
      }

      public async Task<List<T>> GetAllPaginatedAsync(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
      {
         var query = _domainObjects.AsQueryable();

         return await query.Distinct().ToListAsync();
      }

      public Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
      {
         throw new NotImplementedException();
      }

      public void Dispose()
      {
         
      }

      #endregion

      #region Private Methods

      public void CopyProperties(T source, T destination)
      {
         Type sourceType = typeof(T);
         Type destinationType = typeof(T);

         PropertyInfo[] properties = sourceType.GetProperties();

         foreach (PropertyInfo property in properties)
         {
            object value = property.GetValue(source);

            property.SetValue(destination, value);
         }
      }      

      #endregion
   }
}
