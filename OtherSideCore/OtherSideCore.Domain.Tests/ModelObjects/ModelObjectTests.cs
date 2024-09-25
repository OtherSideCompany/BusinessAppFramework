using Moq;
using OtherSideCore.Domain.ModelObjects;
using OtherSideCore.Domain.Services;
using OtherSideCore.Infrastructure.DatabaseFields;
using OtherSideCore.Infrastructure.Entities;

namespace OtherSideCore.Domain.Tests.ModelObjects
{
   public class ModelObjectTests
   {
      IGlobalDataService _globalDataService = new Mock<IGlobalDataService>().Object;

      [Fact]
      public async Task LoadPropertiesFromEntity_PropertiesWellSet()
      {
         var sooner = DateTime.Now.AddDays(-1);
         var evenSooner = DateTime.Now.AddDays(-1);

         var modelObject = new DefaultModelObect();
         var entity = new DefaultEntity()
         {
            Id = 10,
            CreatedById = 7,
            LastModifiedById = 3,
            CreationDate = evenSooner,
            LastModifiedDateTime = sooner
         };

         await modelObject.LoadPropertiesFromEntityAsync(entity, false);

         Assert.Equal(entity.Id, modelObject.Id.Value);
         Assert.Equal(entity.CreatedById, modelObject.CreatedById.Value);
         Assert.Equal(entity.LastModifiedById, modelObject.LastModifiedById.Value);
         Assert.Equal(entity.CreationDate, modelObject.CreationDate.Value);
         Assert.Equal(entity.LastModifiedDateTime, modelObject.LastModifiedDateTime.Value);
      }

      [Fact]
      public async Task LoadPropertiesFromEntity_PropertiesWellSetWithNullTracking()
      {
         var sooner = DateTime.Now.AddDays(-1);
         var evenSooner = DateTime.Now.AddDays(-1);

         var modelObject = new DefaultModelObect();
         var entity = new DefaultEntity()
         {
            Id = 10,
            CreatedById = null,
            LastModifiedById = null,
            CreationDate = evenSooner,
            LastModifiedDateTime = sooner
         };

         await modelObject.LoadPropertiesFromEntityAsync(entity, false);

         Assert.Equal(entity.Id, modelObject.Id.Value);
         Assert.Equal(entity.CreatedById, modelObject.CreatedById.Value);
         Assert.Equal(entity.LastModifiedById, modelObject.LastModifiedById.Value);
         Assert.Equal(entity.CreationDate, modelObject.CreationDate.Value);
         Assert.Equal(entity.LastModifiedDateTime, modelObject.LastModifiedDateTime.Value);
      }

      [Fact]
      public async Task LoadPropertiesFromEntity_UnrecognizedPropertyThrowException()
      {
         var sooner = DateTime.Now.AddDays(-1);
         var evenSooner = DateTime.Now.AddDays(-1);

         var modelObject = new ModelObjectWithUnrecognizedDatabaseField();
         var entity = new EntityWithUnrecognizedDatabaseField();

         await Assert.ThrowsAsync<ArgumentException>(async () => await modelObject.LoadPropertiesFromEntityAsync(entity, false));
      }

      [Fact]
      public async Task LoadPropertiesFromEntity_UsersAreWellSet()
      {
         var sooner = DateTime.Now.AddDays(-1);
         var evenSooner = DateTime.Now.AddDays(-1);

         var modelObject = new DefaultModelObect();

         modelObject.SetServices(new ModelObjectFactory(), _globalDataService);

         var entity = new DefaultEntity()
         {
            Id = 10,
            CreatedById = 7,
            CreatedBy = new Infrastructure.Entities.User() { Id = 7 },
            LastModifiedById = 3,
            LastModifiedBy = new Infrastructure.Entities.User() { Id = 3 },
            CreationDate = evenSooner,
            LastModifiedDateTime = sooner
         };

         await modelObject.LoadPropertiesFromEntityAsync(entity);

         Assert.Equal(7, modelObject.CreatedBy.Id.Value);
         Assert.Equal(3, modelObject.LastModifiedBy.Id.Value);
      }

      [Fact]
      public void SetProperty_ChangesCanBeSavedOrCanceled()
      {
         var modelObject = new DefaultModelObect();

         Assert.False(modelObject.CanSaveChanges());
         Assert.False(modelObject.CanCancelChanges());

         modelObject.LastModifiedDateTime.Value = DateTime.Now;

         Assert.True(modelObject.CanSaveChanges());
         Assert.True(modelObject.CanCancelChanges());
      }

      [Fact]
      public void ResetDbFieldDirtState_ChangesCantBeSavedOrCanceled()
      {
         var modelObject = new DefaultModelObect();

         modelObject.LastModifiedDateTime.Value = DateTime.Now;

         modelObject.ResetDatabaseFieldsDirtyState();

         Assert.False(modelObject.CanSaveChanges());
         Assert.False(modelObject.CanCancelChanges());
      }

      [Fact]
      public void LockDatabasePropertiesEdition_DatabaseFieldsCannotBeEdited()
      {
         var modelObject = new DefaultModelObect();

         var sooner = DateTime.Now.AddDays(-1);

         modelObject.LastModifiedDateTime.Value = sooner;

         modelObject.LockDatabasePropertiesEdition();

         modelObject.LastModifiedDateTime.Value = DateTime.Now;

         Assert.Equal(sooner, modelObject.LastModifiedDateTime.Value);
         Assert.True(modelObject.GetDatabaseFields().All(dbField => !dbField.IsEditable));
      }

      [Fact]
      public void UnlockDatabasePropertiesEdition_DatabaseFieldsCanBeEdited()
      {
         var modelObject = new DefaultModelObect();

         var sooner = DateTime.Now.AddDays(-1);
         var now = DateTime.Now;

         modelObject.LastModifiedDateTime.Value = sooner;

         modelObject.LockDatabasePropertiesEdition();

         modelObject.LastModifiedDateTime.Value = now;

         Assert.Equal(sooner, modelObject.LastModifiedDateTime.Value);

         modelObject.UnlockDatabasePropertiesEdition();

         modelObject.LastModifiedDateTime.Value = now;

         Assert.Equal(now, modelObject.LastModifiedDateTime.Value);
         Assert.True(modelObject.GetDatabaseFields().All(dbField => dbField.IsEditable));
      }

      [Fact]
      public void GetDatabaseFields_AllDatabaseFieldsAreReturned()
      {
         var modelObject = new DefaultModelObect();

         var databaseFields = modelObject.GetDatabaseFields();

         Assert.Equal(5, databaseFields.Count);
      }

      [Fact]
      public void Equals_TwoModelObjectsWithSameIdAreEqual()
      {
         var modelObject1 = new DefaultModelObect();
         var modelObject2 = new DefaultModelObect();

         modelObject1.Id.Value = 10;
         modelObject2.Id.Value = 10;

         Assert.True(modelObject1.Equals(modelObject2));
      }

      [Fact]
      public void Equals_TwoModelObjectsWithDifferentIdAreNotEqual()
      {
         var modelObject1 = new DefaultModelObect();
         var modelObject2 = new DefaultModelObect();

         modelObject1.Id.Value = 10;
         modelObject2.Id.Value = 9;

         Assert.False(modelObject1.Equals(modelObject2));
      }

      [Fact]
      public void Equals_TwoModelObjectsWithSameGuidAreEqual()
      {
         var modelObject1 = new DefaultModelObect();
         var modelObject2 = new DefaultModelObect();

         modelObject2.guid = modelObject1.guid;

         Assert.True(modelObject1.Equals(modelObject2));
      }

      [Fact]
      public void Equals_TwoModelObjectsWithDifferentGuidAreNotEqual()
      {
         var modelObject1 = new DefaultModelObect();
         var modelObject2 = new DefaultModelObect();

         Assert.False(modelObject1.Equals(modelObject2));
      }

      private class DefaultModelObect : ModelObject
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

      private class EntityWithUnrecognizedDatabaseField : EntityBase
      {
         public UnrecognizedInfrastructureDatabaseField UnrecognizedInfrastructureDatabaseField { get; set; }

         public EntityWithUnrecognizedDatabaseField()
         {
            UnrecognizedInfrastructureDatabaseField = new UnrecognizedInfrastructureDatabaseField("UnrecognizedDatabaseField");
         }

         public override List<DatabaseField> GetDatabaseFieldProperties()
         {
            var databaseFields = base.GetDatabaseFieldProperties();

            databaseFields.Add(UnrecognizedInfrastructureDatabaseField);

            return databaseFields;
         }
      }

      private class ModelObjectWithUnrecognizedDatabaseField : ModelObject
      {
         public UnrecognizedDomainDatabaseField UnrecognizedDomainDatabaseField { get; set; }

         public ModelObjectWithUnrecognizedDatabaseField()
         {
            UnrecognizedDomainDatabaseField = new UnrecognizedDomainDatabaseField("UnrecognizedDatabaseField");
         }
      }

      public class UnrecognizedDomainDatabaseField : Domain.DatabaseFields.DatabaseField
      {
         public UnrecognizedDomainDatabaseField(string databaseFieldName) : base(databaseFieldName)
         {

         }

         public override void LoadValue(object value)
         {
            
         }
      }

      public class UnrecognizedInfrastructureDatabaseField : DatabaseField
      {
         public UnrecognizedInfrastructureDatabaseField(string databaseFieldName) : base(databaseFieldName)
         {
            
         }

         public override string GetFormattedValue()
         {
            return String.Empty;
         }
      }
   }
}
