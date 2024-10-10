using Microsoft.EntityFrameworkCore;
using OtherSideCore.Domain.RepositoryInterfaces;
using OtherSideCore.Infrastructure.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace OtherSideCore.Infrastructure.Repositories
{
   public class UserCredentialsRepository<TEntity> : IUserCredentialsRepository where TEntity : User
   {
      #region Fields

      protected IDbContextFactory<DbContext> _dbContextFactory { get; set; }

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public UserCredentialsRepository(IDbContextFactory<DbContext> dbContextFactory)
      {
         _dbContextFactory = dbContextFactory;
      }

      #endregion

      #region Public Methods

      public async Task<(int, string)> GetUserPasswordHashAsync(string userName)
      {
         using (var context = _dbContextFactory.CreateDbContext())
         {
            var entity = await context.Set<TEntity>().Where(u => u.UserName.Equals(userName)).FirstOrDefaultAsync();

            if (entity != null)
            {
               return (entity.Id, entity.PasswordHash);
            }
            else
            {
               return (0, string.Empty);
            }
         }
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
