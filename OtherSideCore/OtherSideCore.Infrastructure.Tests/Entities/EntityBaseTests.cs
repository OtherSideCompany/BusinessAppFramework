using OtherSideCore.Infrastructure.DatabaseFields;
using OtherSideCore.Infrastructure.Entities;
using System.Reflection;

namespace OtherSideCore.Infrastructure.Tests.Entities
{
   public class UnitTest1
   {
      [Fact]
      public void SetProperties_ShouldSetPropertiesCorrectly()
      {
         var entity = new EntityWithUnrecognizedField();

         Assert.NotEqual(1, entity.Id);
         Assert.NotEqual(DateTime.Now, entity.CreationDate);
         Assert.NotEqual(2, entity.CreatedById);
         Assert.NotEqual(DateTime.Now, entity.LastModifiedDateTime);
         Assert.NotEqual(3, entity.LastModifiedById);

         var databaseFields = new List<DatabaseField>
               {
                   new IntegerDatabaseField(1, nameof(EntityBase.Id)),
                   new DateTimeDatabaseField(DateTime.Now, nameof(EntityBase.CreationDate)),
                   new IntegerDatabaseField(2, nameof(EntityBase.CreatedById)),
                   new DateTimeDatabaseField(DateTime.Now, nameof(EntityBase.LastModifiedDateTime)),
                   new IntegerDatabaseField(3, nameof(EntityBase.LastModifiedById))
               };

         entity.SetProperties(databaseFields);

         Assert.Equal(1, entity.Id);
         Assert.Equal(((DateTimeDatabaseField)databaseFields[1]).Value, entity.CreationDate);
         Assert.Equal(2, entity.CreatedById);
         Assert.Equal(((DateTimeDatabaseField)databaseFields[3]).Value, entity.LastModifiedDateTime);
         Assert.Equal(3, entity.LastModifiedById);
      }

      [Fact]
      public void SetProperties_ShouldThrowException_WhenPropertyNotFound()
      {
         var entity = new EntityWithUnrecognizedField();
         var databaseFields = new List<DatabaseField> { new UnrecognizedDatabaseField("RandomField", "RandomValue") };

         Assert.Throws<ArgumentException>(() => entity.SetProperties(databaseFields));
      }

      [Fact]
      public void SetProperties_ShouldThrowException_WhenUnrecognizedType()
      {
         var entity = new EntityWithUnrecognizedField();
         var databaseFields = new List<DatabaseField> { new UnrecognizedDatabaseField(nameof(UnrecognizedDatabaseField), "UnrecognizedValue") };

         Assert.Throws<ArgumentException>(() => entity.SetProperties(databaseFields));
      }

      [Fact]
      public void GetDatabaseFieldProperties_ReturnsCorrectDatabaseFields()
      {
         var entity = new EntityWithUnrecognizedField();

         var propertyCount = entity.GetType().GetProperties().Where(p => !p.GetGetMethod().IsVirtual).Count();

         Assert.Equal(propertyCount, entity.GetDatabaseFieldProperties().Count);
      }      

      private class EntityWithUnrecognizedField : EntityBase
      {
         
      }

      private class UnrecognizedDatabaseField : DatabaseField
      {
         public UnrecognizedDatabaseField(string fieldName, object value) : base(fieldName)
         {
         }

         public override string GetFormattedValue()
         {
            return "";
         }
      }
   }
}