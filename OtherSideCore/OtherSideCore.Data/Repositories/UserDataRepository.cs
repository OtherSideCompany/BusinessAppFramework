using Microsoft.EntityFrameworkCore;
using OtherSideCore.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace OtherSideCore.Data.Repositories
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

      public UserDataRepository(IDbContextFactory<DbContext> dbContextFactory) : base(dbContextFactory)
      {

      }

      #endregion

      #region Methods

      public override async Task<List<T>> GetAllAsync(List<string> filters, bool extendedSearch, CancellationToken cancellationToken)
      {
         using (var context = _dbContextFactory.CreateDbContext())
         {
            var query = context.Set<T>()
                               .Where(u => !u.IsSuperAdmin);

            if (filters != null)
            {
               foreach (var filter in filters)
               {
                  var lowerFilter = filter.ToLower();
                  var maxSearchDistance = Utils.GetMaxSearchDistance(lowerFilter);

                  if (extendedSearch)
                  {
                     query = query.Where(u => Utils.EditDistance(lowerFilter, u.FirstName.ToLower()) <= maxSearchDistance ||
                                              Utils.EditDistance(lowerFilter, u.LastName.ToLower()) <= maxSearchDistance);
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

      public async Task<T> GetSuperAdminUserAsync()
      {
         using (var context = _dbContextFactory.CreateDbContext())
         {
            return await context.Set<T>().FirstOrDefaultAsync(u => u.IsSuperAdmin);
         }
      }

      #endregion
   }
}
