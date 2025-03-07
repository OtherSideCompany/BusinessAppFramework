using Microsoft.EntityFrameworkCore;
using OtherSideCore.Application;
using OtherSideCore.Infrastructure.Entities;
using System.Linq;
using System.Reflection;

namespace OtherSideCore.Infrastructure.Context
{
   public abstract class OtherSideCoreDbContext : DbContext
   {
      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
         modelBuilder.HasDbFunction(typeof(Utils).GetMethod(nameof(Utils.EditDistance), new[] { typeof(string), typeof(string), typeof(int) })).HasName("EditDistance");
      }
   }
}

