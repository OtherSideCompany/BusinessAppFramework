using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Infrastructure.Seed
{
    public interface ISeedContributor
    {
        Task SeedDatabaseAsync(DbContext dbContext);
    }
}
