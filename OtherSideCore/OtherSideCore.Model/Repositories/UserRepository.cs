using Microsoft.EntityFrameworkCore;
using OtherSideCore.Data;
using OtherSideCore.Model.ModelObjects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OtherSideCore.Model.Repositories
{
   public abstract class UserRepository<T, U, V> : Repository<T, U, V>, IUserRepository<T> where T : User, new() 
                                                                                           where U : Data.Entities.User, new()
                                                                                           where V : DbContext                          
   {
      #region Fields

      protected Data.Repositories.UserDataRepository<U, V> _userRepository;

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public UserRepository(Data.Repositories.UserDataRepository<U, V> userRepository) : base(userRepository)
      {
         _userRepository = userRepository;
      }

      #endregion

      #region Methods

      public async Task<List<T>> GetAllAsync(List<string> filters, bool extendedSearch, CancellationToken cancellationToken)
      {
         var users = new List<T>();
         var userEntities = await _userRepository.GetAllAsync(filters, extendedSearch, cancellationToken);

         foreach (var userEntity in userEntities)
         {
            var user = new T();
            await user.LoadPropertiesFromEntityAsync(userEntity);
            users.Add(user);
         }

         return users;
      }

      public async Task<T> GetSuperAdminUserAsync()
      {
         var superAdminUserEntity = await _userRepository.GetSuperAdminUserAsync();
         var user = new T();
         await user.LoadPropertiesFromEntityAsync(superAdminUserEntity);
         return user;
      }

      #endregion
   }
}
