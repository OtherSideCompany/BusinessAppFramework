using Microsoft.EntityFrameworkCore;
using OtherSideCore.Application.DomainObjectBrowser;
using OtherSideCore.Domain.RepositoryInterfaces;
using OtherSideCore.Infrastructure;
using OtherSideCore.Infrastructure.Repositories;

namespace OtherSideCore.Application.Tests.Services
{
    public class DefaultEntityRepository : MockRepository<DefaultDomainObject>
   {
      #region Fields



      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DefaultEntityRepository()
      {

      }

      #endregion

      #region Public Methods

      public override async Task<List<DefaultDomainObject>> GetAllAsync(List<string> filters, List<Constraint<DefaultDomainObject>> constraints, bool extendedSearch, CancellationToken cancellationToken)
      {
         var query = _domainObjects.AsQueryable();

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
