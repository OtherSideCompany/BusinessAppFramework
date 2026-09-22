using BusinessAppFramework.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessAppFramework.Infrastructure.Context
{
    public class ModuleSettingsModelBuilderContributor : IModelBuilderContributor
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ModuleSettings>().HasIndex(e => e.Key).IsUnique();
        }
    }
}
