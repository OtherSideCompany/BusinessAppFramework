using BusinessAppFramework.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessAppFramework.Infrastructure.Context
{
    public class UserSettingsModelBuilderContributor : IModelBuilderContributor
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserSettings>().HasIndex(e => new { e.UserId, e.Key }).IsUnique();
        }
    }
}
