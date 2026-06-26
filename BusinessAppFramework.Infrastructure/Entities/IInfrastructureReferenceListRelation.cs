using BusinessAppFramework.Application.Relations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessAppFramework.Infrastructure.Entities
{
    public interface IInfrastructureReferenceListRelation : IReferenceListRelationEntry
    {
        IQueryable<int> GetTargetIds(DbContext context, int parentId);
    }
}
