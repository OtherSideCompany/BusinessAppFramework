using Microsoft.EntityFrameworkCore;

namespace BusinessAppFramework.Infrastructure.Context
{
   public interface IModelBuilderContributor
   {
      void Build(ModelBuilder modelBuilder);
   }
}
