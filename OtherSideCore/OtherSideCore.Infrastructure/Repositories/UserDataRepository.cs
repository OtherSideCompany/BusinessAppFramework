using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Logging;
using OtherSideCore.Infrastructure;
using OtherSideCore.Infrastructure.Entities;

namespace OtherSideCore.Infrastructure.Repositories
{
   public class UserDataRepository<T> : DataRepository<T>, IUserDataRepository<T> where T : User, new()
   {
      #region Fields



      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public UserDataRepository(IDbContextFactory<DbContext> dbContextFactory, ILoggerFactory loggerFactory) : base(dbContextFactory, loggerFactory)
      {

      }

      #endregion

      #region Public Methods

      public override async Task<List<T>> GetAllAsync(List<string> filters, bool extendedSearch, CancellationToken cancellationToken)
      {
         LogGetAllAsync(filters, extendedSearch);

         using (var context = _dbContextFactory.CreateDbContext())
         {
            var query = context.Set<T>().AsQueryable();

            if (filters != null)
            {
               foreach (var filter in filters)
               {
                  var lowerFilter = filter.ToLower();
                  var maxSearchDistance = Utils.GetMaxSearchDistance(lowerFilter);

                  if (extendedSearch)
                  {
                     query = query.Where(u => Utils.EditDistance(lowerFilter, u.FirstName.ToLower(), maxSearchDistance) <= maxSearchDistance ||
                                              Utils.EditDistance(lowerFilter, u.LastName.ToLower(), maxSearchDistance) <= maxSearchDistance);
                  }
                  else
                  {
                     query = query.Where(u => u.FirstName.ToLower().Contains(lowerFilter) ||
                                              u.LastName.ToLower().Contains(lowerFilter));
                  }
               }
            }

            return await query.Distinct().ToListAsync(cancellationToken);
         }
      }

      public virtual async Task<(int, string)> GetUserPasswordHashAsync(string userName)
      {
         _logger.LogInformation("{Type}, {MethodName}, user name : {userName}", GetType(), nameof(GetUserPasswordHashAsync), userName);

         using (var context = _dbContextFactory.CreateDbContext())
         {
            var entity = await context.Set<T>().Where(u => u.UserName.Equals(userName)).FirstOrDefaultAsync();

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
   }
}
