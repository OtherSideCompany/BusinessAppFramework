using BusinessAppFramework.Application.Relations;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BusinessAppFramework.Infrastructure.Entities
{
   internal interface IInfrastructureParentChildRelation : IParentChildRelationEntry
   {
      IQueryable<int> GetChildrenIds(DbContext context, int parentId);
   }
}
