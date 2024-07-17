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
         List<T> users = new List<T>();

         using (var context = _dbContextFactory.CreateDbContext())
         {
            if (filters != null && filters.Any())
            {
               if (extendedSearch)
               {
                  foreach (var filter in filters)
                  {
                     var lowerFilter = filter.ToLower();
                     var maxSearchDistance = Utils.GetMaxSearchDistance(lowerFilter);

                     users.AddRange(await context.Set<T>()
                                                 .Where(u => (Utils.EditDistance(lowerFilter, u.FirstName.ToLower()) <= maxSearchDistance ||
                                                              Utils.EditDistance(lowerFilter, u.LastName.ToLower()) <= maxSearchDistance) &&
                                                              !u.IsSuperAdmin)
                                                 .ToListAsync(cancellationToken));
                  }
               }
               else
               {
                  foreach (var filter in filters)
                  {
                     var lowerFilter = filter.ToLower();

                     users.AddRange(await context.Set<T>()
                                                 .Where(u => (u.FirstName.ToLower().Contains(lowerFilter) ||
                                                              u.LastName.ToLower().Contains(lowerFilter)) &&
                                                              !u.IsSuperAdmin)
                                                 .ToListAsync(cancellationToken));
                  }
               }
            }
            else
            {
               users = await context.Set<T>()
                                    .Where(u => !u.IsSuperAdmin).ToListAsync(cancellationToken);
            }
         }

         return users.DistinctBy(u => u.Id).ToList();
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
