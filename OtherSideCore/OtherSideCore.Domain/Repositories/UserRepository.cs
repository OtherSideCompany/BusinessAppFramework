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

      private IUserDataRepository<U> _userDataRepository { get => (IUserDataRepository<U>)_entityDataRepository;}

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
         modelObject.CreatedBy = await GetAsync(modelObject.CreatedById.Value, cancellationToken);
         modelObject.LastModifiedBy = await GetAsync(modelObject.LastModifiedById.Value, cancellationToken);
      }

      public async Task<T> GetUserByCredentials(string userName, string passwordHash)
      {
         var entity = await _userDataRepository.GetUserByCredentials(userName, passwordHash);

         if (entity != null)
         {
            var modelObject = new T();
            modelObject.SetModelObjectFactory(_modelObjectFactory);

            await modelObject.LoadPropertiesFromEntityAsync(entity);

            return modelObject;
         }
         else
         {
            return null;
         }
      }

      #endregion
   }
}
