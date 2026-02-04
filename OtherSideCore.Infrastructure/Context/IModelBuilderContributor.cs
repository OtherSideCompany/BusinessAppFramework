using Microsoft.EntityFrameworkCore;

namespace OtherSideCore.Infrastructure.Context
{
   public interface IModelBuilderContributor
   {
      void Build(ModelBuilder modelBuilder);
   }
}
