using Microsoft.EntityFrameworkCore;
using OtherSideCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using OtherSideCore.Data.Repositories;

namespace OtherSideCore.Model.Repositories
{
    public abstract class Repository<T, U, V> : IRepository<T>, IDisposable where T : ModelObject, new() 
                                                                            where U : EntityBase, new()
                                                                            where V : DbContext
   {
      #region Fields

      protected DataRepository<U, V> _entityDataRepository;

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public Repository(DataRepository<U, V> repository)
      {
         _entityDataRepository = repository;
      }

      #endregion

      #region Methods

      public async Task<T> GetAsync(int id, CancellationToken cancellationToken)
      {
         var user = new T();
         var userEntity = await _entityDataRepository.GetAsync(id, cancellationToken);
         await user.LoadPropertiesFromEntityAsync(userEntity);
         return user;
      }

      public async Task LoadAsync(ModelObject modelObject)
      {
         modelObject.LockDatabasePropertiesEdition();

         var userEntity = await _entityDataRepository.GetAsync(modelObject.Id.Value, CancellationToken.None);
         await modelObject.LoadPropertiesFromEntityAsync(userEntity);

         modelObject.UnlockDatabasePropertiesEdition();

         modelObject.ResetDatabaseFieldsDirtyState();
      }

      public async Task SaveAsync(ModelObject modelObject, int userId)
      {
         modelObject.LastModifiedById.Value = userId;
         modelObject.LastModifiedDateTime.Value = DateTime.Now;

         if (modelObject.Id.Value == 0)
         {
            modelObject.CreatedById.Value = userId;
            modelObject.CreationDate.Value = DateTime.Now;

            modelObject.Id.Value = await _entityDataRepository.CreateAsync(modelObject.ConvertPropertiesToDataProperties());
         }
         else
         {
            modelObject.LockDatabasePropertiesEdition();

            await _entityDataRepository.SaveAsync(modelObject.Id.Value, modelObject.ConvertDirtyPropertiesToDataProperties());

            modelObject.UnlockDatabasePropertiesEdition();
         }

         modelObject.ResetDatabaseFieldsDirtyState();
      }

      public async Task DeleteAsync(ModelObject modelObject)
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
