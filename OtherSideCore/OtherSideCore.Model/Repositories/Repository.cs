using Microsoft.EntityFrameworkCore;
using OtherSideCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using OtherSideCore.Data.Repositories;
using OtherSideCore.Model.ModelObjects;

namespace OtherSideCore.Model.Repositories
{
   public class Repository<T, U> : IRepository<T>, IDisposable where T : ModelObject, new()
                                                               where U : EntityBase, new()
   {
      #region Fields

      protected IDataRepository<U> _entityDataRepository;
      protected IModelObjectFactory _modelObjectFactory;

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public Repository(IDataRepository<U> repository, IModelObjectFactory modelObjectFactory)
      {
         _entityDataRepository = repository;
         _modelObjectFactory = modelObjectFactory;
      }

      #endregion

      #region Methods

      public async Task<List<T>> GetAllAsync(List<string> filters, bool extendedSearch, CancellationToken cancellationToken)
      {
         var modelObjects = new List<T>();
         var entities = await _entityDataRepository.GetAllAsync(filters, extendedSearch, cancellationToken);

         foreach (var entity in entities)
         {
            var modelObject = new T();
            modelObject.SetModelObjectFactory(_modelObjectFactory);
            await modelObject.LoadPropertiesFromEntityAsync(entity);
            modelObjects.Add(modelObject);
         }

         return modelObjects;
      }

      public async Task<T> GetAsync(int id, CancellationToken cancellationToken)
      {
         var modelObject = new T();
         modelObject.SetModelObjectFactory(_modelObjectFactory);
         var entity = await _entityDataRepository.GetAsync(id, cancellationToken);
         await modelObject.LoadPropertiesFromEntityAsync(entity);
         return modelObject;
      }

      public async Task LoadAsync(T modelObject)
      {
         var entity = await _entityDataRepository.GetAsync(modelObject.Id.Value, CancellationToken.None);
         await modelObject.LoadPropertiesFromEntityAsync(entity);

         modelObject.ResetDatabaseFieldsDirtyState();
      }

      public async Task<T> CreateAsync(int userId)
      {
         var modelObject = new T();
         modelObject.SetModelObjectFactory(_modelObjectFactory);
         return await CreateAsync(modelObject, userId);
      }

      private async Task<T> CreateAsync(T modelObject, int userId)
      {
         modelObject.LastModifiedById.Value = userId;
         modelObject.LastModifiedDateTime.Value = DateTime.Now;

         modelObject.CreatedById.Value = userId;
         modelObject.CreationDate.Value = DateTime.Now;

         modelObject.Id.Value = await _entityDataRepository.CreateAsync(modelObject.ConvertPropertiesToDataProperties());

         return modelObject;
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
         modelObject.Id.Value = 0;

         modelObject.UnlockDatabasePropertiesEdition();
      }

      public void Dispose()
      {
         _entityDataRepository.Dispose();
      }

      #endregion
   }
}
