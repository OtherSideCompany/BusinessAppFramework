using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
   public interface IModelBuilderContributor
   {
      void Build(ModelBuilder modelBuilder);
   }
}
