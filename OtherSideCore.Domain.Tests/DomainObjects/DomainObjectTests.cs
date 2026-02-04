using OtherSideCore.Domain.DomainObjects;
using OtherSideCore.Infrastructure.Entities;

namespace OtherSideCore.Domain.Tests.DomainObjects
{
   public class DomainObjectTests
   {
      [Fact]
      public void Equals_TwoModelObjectsWithSameIdAreEqual()
      {
         var modelObject1 = new DefaultModelObect();
         var modelObject2 = new DefaultModelObect();

         modelObject1.Id = 10;
         modelObject2.Id = 10;

         Assert.True(modelObject1.Equals(modelObject2));
      }

      [Fact]
      public void Equals_TwoModelObjectsWithDifferentIdAreNotEqual()
      {
         var modelObject1 = new DefaultModelObect();
         var modelObject2 = new DefaultModelObect();

         modelObject1.Id = 10;
         modelObject2.Id = 9;

         Assert.False(modelObject1.Equals(modelObject2));
      }

      [Fact]
      public void Equals_TwoModelObjectsWithSameHashCodeAreEqual()
      {
         var modelObject1 = new DefaultModelObect();
         var modelObject2 = new DefaultModelObect();

         modelObject1 = modelObject2;

         Assert.True(modelObject1.Equals(modelObject2));
         Assert.Equal(modelObject1.GetHashCode(), modelObject2.GetHashCode());
      }

      [Fact]
      public void Equals_TwoModelObjectsWithDifferentGuidAreNotEqual()
      {
         var modelObject1 = new DefaultModelObect();
         var modelObject2 = new DefaultModelObect();

         Assert.False(modelObject1.Equals(modelObject2));
         Assert.NotEqual(modelObject1.GetHashCode(), modelObject2.GetHashCode());
      }

      private class DefaultModelObect : DomainObject
      {
         public DefaultModelObect()
         {
            
         }
      }

      private class DefaultEntity : EntityBase
      {
         public DefaultEntity()
         {
            
         }
      }
   }
}
