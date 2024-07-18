using Microsoft.EntityFrameworkCore;
using OtherSideCore.Data;
using OtherSideCore.Model.ModelObjects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OtherSideCore.Model.Repositories
{
    public class UserRepository<T, U> : Repository<T, U>, IUserRepository<T> where T : User, new() 
                                                                            where U : Data.Entities.User, new()                         
   {
      #region Fields

      protected Data.Repositories.IUserDataRepository<U> _userRepository;

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public UserRepository(Data.Repositories.IUserDataRepository<U> userRepository, IModelObjectFactory modelObjectFactory) : base(userRepository, modelObjectFactory)
      {
         _userRepository = userRepository;
      }

      #endregion

      #region Methods

      public async Task<T> GetSuperAdminUserAsync()
      {
         var superAdminUserEntity = await _userRepository.GetSuperAdminUserAsync();
         var user = new T();
         user.SetModelObjectFactory(_modelObjectFactory);
         await user.LoadPropertiesFromEntityAsync(superAdminUserEntity);
         return user;
      }

      public async Task LoadCreatorAndModificator(ModelObject modelObject, CancellationToken cancellationToken)
      {
         modelObject.CreatedBy = await GetAsync(modelObject.CreatedById.Value, cancellationToken);
         modelObject.LastModifiedBy = await GetAsync(modelObject.LastModifiedById.Value, cancellationToken);
      }

      #endregion
   }
}
