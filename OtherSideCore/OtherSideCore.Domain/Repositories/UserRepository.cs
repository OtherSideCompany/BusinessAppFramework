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

      private IUserDataRepository<U> _userDataRepository { get => (IUserDataRepository<U>)_entityDataRepository; }

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public UserRepository(IUserDataRepository<U> userRepository, IModelObjectFactory modelObjectFactory) : base(userRepository, modelObjectFactory)
      {

      }

      #endregion

      #region Public Methods

      public async Task LoadCreatorAndModificator(ModelObject modelObject, CancellationToken cancellationToken)
      {

         modelObject.CreatedBy = modelObject.CreatedById.Value.HasValue ? await GetAsync((int)modelObject.CreatedById.Value, cancellationToken) : null;
         modelObject.LastModifiedBy = modelObject.LastModifiedById.Value.HasValue ? await GetAsync((int)modelObject.LastModifiedById.Value, cancellationToken) : null;
      }

      public async Task<(int, string)> GetUserPasswordHashAsync(string userName)
      {
         return await _userDataRepository.GetUserPasswordHashAsync(userName);
      }

      #endregion
   }
}
