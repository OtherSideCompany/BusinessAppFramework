using OtherSideCore.Model.DatabaseFields;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using OtherSideCore.Model.ModelObjects;
using OtherSideCore.Data;
using OtherSideCore.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace OtherSideCore.Model
{
    public abstract class ModelObject : ObservableObject, IDisposable
   {
      #region Fields

      private IntegerDatabaseField m_Id;
      private DateTimeDatabaseField m_CreationDate;
      private IntegerDatabaseField m_CreatedById;
      private DateTimeDatabaseField m_LastModifiedDateTime;
      private IntegerDatabaseField m_LastModifiedById;

      private ModelObjects.User m_CreatedBy;
      private ModelObjects.User m_LastModifiedBy;

      private bool m_IsDirty;

      #endregion

      #region Properties

      public Guid guid { get; set; }

      public IntegerDatabaseField Id
      {
         get => m_Id;
         private set { SetProperty(ref m_Id, value); OnPropertyChanged(nameof(IsCreated)); }
      }

      public DateTimeDatabaseField CreationDate
      {
         get => m_CreationDate;
         private set { SetProperty(ref m_CreationDate, value); }
      }

      public IntegerDatabaseField CreatedById
      {
         get => m_CreatedById;
         private set { SetProperty(ref m_CreatedById, value); }
      }

      public DateTimeDatabaseField LastModifiedDateTime
      {
         get => m_LastModifiedDateTime;
         private set { SetProperty(ref m_LastModifiedDateTime, value); }
      }

      public IntegerDatabaseField LastModifiedById
      {
         get => m_LastModifiedById;
         private set { SetProperty(ref m_LastModifiedById, value); }
      }

      public bool IsCreated
      {
         get
         {
            return Id.Value != 0;
         }
      }

      public ModelObjects.User CreatedBy 
      { 
         get => m_CreatedBy;
         protected set { SetProperty(ref m_CreatedBy, value); }
      }

      public ModelObjects.User LastModifiedBy
      {
         get => m_LastModifiedBy;
         protected set { SetProperty(ref m_LastModifiedBy, value); }
      }

      #endregion

      #region Constructor

      public ModelObject()
      {
         guid = Guid.NewGuid();

         Id = new IntegerDatabaseField("Id");
         CreationDate = new DateTimeDatabaseField("CreationDate");
         CreatedById = new IntegerDatabaseField("CreatedById");
         LastModifiedDateTime = new DateTimeDatabaseField("LastModifiedDateTime");
         LastModifiedById = new IntegerDatabaseField("LastModifiedById");
      }

      #endregion

      #region Methods

      public virtual bool MatchFilter(List<string> filters, bool extendedSearch)
      {
         return false;
      }

      internal async Task LoadPropertiesFromEntityAsync(Data.Entities.EntityBase entity)
      {
         var databaseFieldProperties = entity.GetDatabaseFieldProperties();

         foreach (var databaseFieldProperty in databaseFieldProperties)
         {
            PropertyInfo propertyInfo = GetDatabaseFieldsPropertyInfos().First(dbf => (dbf.GetValue(this) as DatabaseField).DatabaseFieldName.Equals(databaseFieldProperty.DatabaseFieldName));
            var databaseField = propertyInfo.GetValue(this);

            switch (databaseField)
            {
               case IntegerDatabaseField integerDatabaseField:
                  integerDatabaseField.LoadValue((databaseFieldProperty as Data.DatabaseFields.IntegerDatabaseField).Value);
                  break;
               case StringDatabaseField stringDatabaseField:
                  stringDatabaseField.LoadValue((databaseFieldProperty as Data.DatabaseFields.StringDatabaseField).Value);
                  break;
               case DateTimeDatabaseField dateTimeDatabaseField:
                  dateTimeDatabaseField.LoadValue((databaseFieldProperty as Data.DatabaseFields.DateTimeDatabaseField).Value);
                  break;
               case DateOnlyDatabaseField dateOnlyDatabaseField:
                  dateOnlyDatabaseField.LoadValue((databaseFieldProperty as Data.DatabaseFields.DateOnlyDatabaseField).Value);
                  break;
               case BoolDatabaseField boolDatabaseField:
                  boolDatabaseField.LoadValue((databaseFieldProperty as Data.DatabaseFields.BoolDatabaseField).Value);
                  break;
               default:
                  throw new Exception("Unrecognized type " + databaseField.GetType());
            }
         }

         await LoadModelObjectPropertiesFromEntityAsync(entity);
      }

      protected virtual async Task LoadModelObjectPropertiesFromEntityAsync(Data.Entities.EntityBase entity) { }

      public bool CanSaveChanges()
      {
         return GetDatabaseFields().Any(dbf => dbf.IsDirty);
      }

      public bool CanCancelChanges()
      {
         return GetDatabaseFields().Any(dbf => dbf.IsDirty);
      }

      internal void LockDatabasePropertiesEdition()
      {
         foreach (var databaseProperty in GetDatabaseFields())
         {
            databaseProperty.IsEditable = false;
         }
      }

      internal void UnlockDatabasePropertiesEdition()
      {
         foreach (var databaseProperty in GetDatabaseFields())
         {
            databaseProperty.IsEditable = true;
         }
      }

      public bool CanBeDeleted()
      {
         return true;
      }

      internal void ResetDatabaseFieldsDirtyState()
      {
         GetDirtyDatabaseFields().ForEach(dbf => dbf.IsDirty = false);
      }

      protected List<PropertyInfo> GetDatabaseFieldsPropertyInfos()
      {
         return GetType().GetProperties().Where(p => p.PropertyType.IsSubclassOf(typeof(DatabaseField))).ToList();
      }

      public List<DatabaseField> GetDatabaseFields()
      {
         return GetType().GetProperties().Where(p => p.PropertyType.IsSubclassOf(typeof(DatabaseField)))
                                         .Select(p => p.GetValue(this))
                                         .Cast<DatabaseField>()
                                         .ToList();

      }

      protected List<DatabaseField> GetDirtyDatabaseFields()
      {
         return GetType().GetProperties().Where(p => p.PropertyType.IsSubclassOf(typeof(DatabaseField)))
                                         .Select(p => p.GetValue(this))
                                         .Cast<DatabaseField>()
                                         .Where(p => (p as DatabaseField).IsDirty)
                                         .ToList();
      }

      internal List<Data.DatabaseFields.DatabaseField> ConvertDirtyPropertiesToDataProperties()
      {
         return ConvertDatabaseFieldsToDataProperties(GetDirtyDatabaseFields());
      }

      internal List<Data.DatabaseFields.DatabaseField> ConvertPropertiesToDataProperties()
      {
         return ConvertDatabaseFieldsToDataProperties(GetDatabaseFields());
      }

      protected Data.DatabaseFields.DatabaseField ConvertDatabaseFieldToDataProperty(DatabaseField databaseField)
      {
         switch (databaseField)
         {
            case StringDatabaseField stringDatabaseField:
               return new Data.DatabaseFields.StringDatabaseField(stringDatabaseField.Value, stringDatabaseField.DatabaseFieldName);
            case IntegerDatabaseField integerDatabaseField:
               return new Data.DatabaseFields.IntegerDatabaseField(integerDatabaseField.Value, integerDatabaseField.DatabaseFieldName);
            case DateTimeDatabaseField dateTimeDatabaseField:
               return new Data.DatabaseFields.DateTimeDatabaseField(dateTimeDatabaseField.Value, dateTimeDatabaseField.DatabaseFieldName);
            case DateOnlyDatabaseField dateOnlyDatabaseField:
               return new Data.DatabaseFields.DateOnlyDatabaseField(dateOnlyDatabaseField.Value, dateOnlyDatabaseField.DatabaseFieldName);
            case BoolDatabaseField boolDatabaseField:
               return new Data.DatabaseFields.BoolDatabaseField(boolDatabaseField.Value, boolDatabaseField.DatabaseFieldName);
            default:
               throw new Exception("Unrecognized type " + databaseField.GetType());
         }
      }

      private List<Data.DatabaseFields.DatabaseField> ConvertDatabaseFieldsToDataProperties(List<DatabaseField> databaseFields)
      {
         var entityDataProperties = new List<OtherSideCore.Data.DatabaseFields.DatabaseField>();

         foreach (var databaseField in databaseFields)
         {
            entityDataProperties.Add(ConvertDatabaseFieldToDataProperty(databaseField));
         }

         return entityDataProperties;
      }

      public override bool Equals(object obj)
      {
         var item = obj as ModelObject;

         if (item == null)
         {
            return false;
         }

         if (Id.Value == 0 && item.Id.Value == 0)
         {
            return guid == item.guid;
         }
         else
         {
            return Id == item.Id;
         }
      }

      public abstract void Dispose();

      #endregion
   }
}
