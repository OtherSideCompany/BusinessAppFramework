using OtherSideCore.Domain.Repositories;
using OtherSideCore.Infrastructure;
using OtherSideCore.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Domain.Tests.Repositories
{
   public class DefaultEntityDataRepository : MockDataRepository<DefaultEntity>
   {
      #region Fields



      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DefaultEntityDataRepository()
      {

      }

      #endregion

      #region Public Methods

      public override async Task<List<DefaultEntity>> GetAllAsync(List<string> filters, List<Constraint<DefaultEntity>> constraints, bool extendedSearch, CancellationToken cancellationToken)
      {
         var query = _entities.AsQueryable();

         if (filters != null)
         {
            foreach (var filter in filters)
            {
               var lowerFilter = filter.ToLower();
               var maxSearchDistance = Utils.GetMaxSearchDistance(lowerFilter);

               if (extendedSearch)
               {
                  query = query.Where(u => Utils.EditDistance(lowerFilter, u.RandomProperty.ToLower(), maxSearchDistance) <= maxSearchDistance);
               }
               else
               {
                  query = query.Where(u => u.RandomProperty.ToLower().Contains(lowerFilter));
               }
            }
         }

         if (constraints != null)
         {
            foreach (var constraint in constraints)
            {
               query = query.Where(constraint.LambdaConstructor());
            }
         }

         return query.Distinct().ToList();
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
