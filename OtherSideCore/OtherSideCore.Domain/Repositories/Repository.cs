using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using OtherSideCore.Domain.ModelObjects;
using OtherSideCore.Infrastructure.Entities;
using OtherSideCore.Infrastructure.Repositories;
using OtherSideCore.Domain.Services;

namespace OtherSideCore.Domain.Repositories
{
   public class Repository<T, U> : IRepository<T>, IDisposable where T : ModelObject, new()
                                                               where U : EntityBase, new()
   {
      #region Fields

      protected IDataRepository<U> _entityDataRepository;
      protected IModelObjectFactory _modelObjectFactory;
      protected IGlobalDataService _globalDataService;

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public Repository(IDataRepository<U> repository, IModelObjectFactory modelObjectFactory, IGlobalDataService globalDataService)
      {
         _entityDataRepository = repository;
         _modelObjectFactory = modelObjectFactory;
         _globalDataService = globalDataService;
      }

      #endregion

      #region Public Methods

      public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken)
      {
         return await GetAllAsync(new List<string>(), new List<Constraint>(), false, cancellationToken);
      }

      public async Task<List<T>> GetAllAsync(List<string> filters, List<Constraint> constraints, bool extendedSearch, CancellationToken cancellationToken)
      {
         var entityConstraints = new List<Constraint<U>>();

         foreach (var constraint in constraints)
         {
            entityConstraints.Add(new Constraint<U>(constraint.PropertyName, constraint.Value));
         }

         return await GetAllAsync(filters, entityConstraints, extendedSearch, cancellationToken);
      }

      public async Task<T> GetAsync(int id, CancellationToken cancellationToken)
      {
         var entity = await _entityDataRepository.GetAsync(id, cancellationToken);

         if (entity != null)
         {
            var modelObject = _modelObjectFactory.CreateModelObject<T>(_globalDataService);

            await modelObject.LoadPropertiesFromEntityAsync(entity);

            return modelObject;
         }
         else
         {
            return null;
         }        
      }

      public async Task LoadAsync(T modelObject)
      {
         var entity = await _entityDataRepository.GetAsync(modelObject.Id.Value, CancellationToken.None);
         await modelObject.LoadPropertiesFromEntityAsync(entity);

         modelObject.ResetDatabaseFieldsDirtyState();
      }

      public async Task<T> CreateAsync(int userId)
      {
         var modelObject = _modelObjectFactory.CreateModelObject<T>(_globalDataService);
         return await CreateAsync(modelObject, userId);
      }

      public async Task SaveAsync(T modelObject, int userId)
      {
         if (modelObject.Id.Value == 0)
         {
            await CreateAsync(modelObject, userId);
         }
         else
         {
            var lastMoficationTime = await _entityDataRepository.GetModificatonTimeAsync(modelObject.Id.Value);

            if (modelObject.LastModifiedDateTime.Value < lastMoficationTime)
            {
               throw new Exception("User do not work on last data for this entity");
            }

            modelObject.LastModifiedById.Value = userId;
            modelObject.LastModifiedDateTime.Value = DateTime.Now;

            modelObject.LockDatabasePropertiesEdition();

            await _entityDataRepository.SaveAsync(modelObject.Id.Value, modelObject.ConvertDirtyPropertiesToDataProperties());

            modelObject.UnlockDatabasePropertiesEdition();
         }

         modelObject.ResetDatabaseFieldsDirtyState();
      }

      public async Task DeleteAsync(T modelObject)
      {
         modelObject.LockDatabasePropertiesEdition();

         await _entityDataRepository.DeleteAsync(modelObject.Id.Value);         

         modelObject.UnlockDatabasePropertiesEdition();

         modelObject.Id.Value = 0;
      }

      public void Dispose()
      {
         _entityDataRepository.Dispose();
      }

      #endregion

      #region Private Methods

      protected async Task<List<T>> GetAllAsync(List<string> filters, List<Constraint<U>> constraints, bool extendedSearch, CancellationToken cancellationToken)
      {
         var modelObjects = new List<T>();
         var entities = await _entityDataRepository.GetAllAsync(filters, constraints, extendedSearch, cancellationToken);

         foreach (var entity in entities)
         {
            var modelObject = _modelObjectFactory.CreateModelObject<T>(_globalDataService);
            await modelObject.LoadPropertiesFromEntityAsync(entity);
            modelObjects.Add(modelObject);
         }

         return modelObjects;
      }

      private async Task<T> CreateAsync(T modelObject, int userId)
      {
         modelObject.LastModifiedById.Value = userId;
         modelObject.LastModifiedDateTime.Value = DateTime.Now;

         modelObject.CreatedById.Value = userId;
         modelObject.CreationDate.Value = DateTime.Now;

         modelObject.Id.Value = await _entityDataRepository.CreateAsync(modelObject.ConvertPropertiesToDataProperties());

         modelObject.ResetDatabaseFieldsDirtyState();

         return modelObject;
      }

      #endregion
   }
}
