using OtherSideCore.Infrastructure.Entities;
using OtherSideCore.Infrastructure.Repositories;
using OtherSideCore.Domain.ModelObjects;
using System.Threading;
using System.Threading.Tasks;

namespace OtherSideCore.Domain.Repositories
{
   public class UserRepository<T, U> : Repository<T, U>, IUserRepository<T> where T : ModelObjects.User, new()
                                                                            where U : Infrastructure.Entities.User, new()
   {
      #region Fields

      protected IUserDataRepository<U> _userRepository;

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public UserRepository(IUserDataRepository<U> userRepository, IModelObjectFactory modelObjectFactory) : base(userRepository, modelObjectFactory)
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
