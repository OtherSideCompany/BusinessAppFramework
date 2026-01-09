using Microsoft.EntityFrameworkCore;
using OtherSideCore.Application;
using OtherSideCore.Application.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OtherSideCore.Infrastructure.Entities
{
    internal interface IInfrastructureParentChildRelation : IParentChildRelationEntry
    {
        IQueryable<int> GetChildrenIds(DbContext context, int parentId);
    }
}
