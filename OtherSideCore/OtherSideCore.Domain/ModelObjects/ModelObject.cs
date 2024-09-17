using OtherSideCore.Domain.DatabaseFields;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using OtherSideCore.Infrastructure.Entities;

namespace OtherSideCore.Domain.ModelObjects
{
   public abstract class ModelObject : ObservableObject, IDisposable
   {
      #region Fields

      protected IModelObjectFactory _modelObjectFactory;

      private IntegerDatabaseField m_Id;
      private DateTimeDatabaseField m_CreationDate;
      private NullableIntegerDatabaseField m_CreatedById;
      private DateTimeDatabaseField m_LastModifiedDateTime;
      private NullableIntegerDatabaseField m_LastModifiedById;

      private User m_CreatedBy;
      private User m_LastModifiedBy;

      protected bool _isLoading;

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

      public NullableIntegerDatabaseField CreatedById
      {
         get => m_CreatedById;
         private set { SetProperty(ref m_CreatedById, value); }
      }

      public DateTimeDatabaseField LastModifiedDateTime
      {
         get => m_LastModifiedDateTime;
         private set { SetProperty(ref m_LastModifiedDateTime, value); }
      }

      public NullableIntegerDatabaseField LastModifiedById
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

      public User CreatedBy
      {
         get => m_CreatedBy;
         set { SetProperty(ref m_CreatedBy, value); }
      }

      public User LastModifiedBy
      {
         get => m_LastModifiedBy;
         set { SetProperty(ref m_LastModifiedBy, value); }
      }

      #endregion

      #region Constructor

      public ModelObject()
      {
         guid = Guid.NewGuid();

         Id = new IntegerDatabaseField("Id");
         CreationDate = new DateTimeDatabaseField(nameof(CreationDate));
         CreatedById = new NullableIntegerDatabaseField(nameof(CreatedById));
         LastModifiedDateTime = new DateTimeDatabaseField(nameof(LastModifiedDateTime));
         LastModifiedById = new NullableIntegerDatabaseField(nameof(LastModifiedById));
      }

      #endregion

      #region Public Methods

      public void SetModelObjectFactory(IModelObjectFactory modelObjectFactory)
      {
         _modelObjectFactory = modelObjectFactory;
      }

      public virtual bool MatchFilter(List<string> filters, bool extendedSearch)
      {
         throw new NotImplementedException();
      }

      public async Task LoadPropertiesFromEntityAsync(EntityBase entity, bool cascade = true)
      {
         _isLoading = true;

         var databaseFieldProperties = entity?.GetDatabaseFieldProperties();

         if (databaseFieldProperties != null)
         {
            foreach (var databaseFieldProperty in databaseFieldProperties)
            {
               PropertyInfo propertyInfo = GetDatabaseFieldsPropertyInfos().First(dbf => (dbf.GetValue(this) as DatabaseField).DatabaseFieldName.Equals(databaseFieldProperty.DatabaseFieldName));
               var databaseField = propertyInfo.GetValue(this);

               switch (databaseField)
               {
                  case IntegerDatabaseField integerDatabaseField:
                     integerDatabaseField.LoadValue((databaseFieldProperty as Infrastructure.DatabaseFields.IntegerDatabaseField).Value);
                     break;
                  case StringDatabaseField stringDatabaseField:
                     stringDatabaseField.LoadValue((databaseFieldProperty as Infrastructure.DatabaseFields.StringDatabaseField).Value);
                     break;
                  case DateTimeDatabaseField dateTimeDatabaseField:
                     dateTimeDatabaseField.LoadValue((databaseFieldProperty as Infrastructure.DatabaseFields.DateTimeDatabaseField).Value);
                     break;
                  case DateOnlyDatabaseField dateOnlyDatabaseField:
                     dateOnlyDatabaseField.LoadValue((databaseFieldProperty as Infrastructure.DatabaseFields.DateOnlyDatabaseField).Value);
                     break;
                  case BoolDatabaseField boolDatabaseField:
                     boolDatabaseField.LoadValue((databaseFieldProperty as Infrastructure.DatabaseFields.BoolDatabaseField).Value);
                     break;
                  case NullableIntegerDatabaseField nullableIntegerDatabaseField:
                     nullableIntegerDatabaseField.LoadValue((databaseFieldProperty as Infrastructure.DatabaseFields.NullableIntegerDatabaseField).Value);
                     break;
                  default:
                     throw new ArgumentException("Unrecognized type " + databaseField.GetType());
               }
            }

            if (cascade)
            {
               await LoadModelObjectPropertiesFromEntityAsync(entity);
            }
         }

         _isLoading = false;
      }

      protected virtual async Task LoadModelObjectPropertiesFromEntityAsync(EntityBase entity)
      {
         if (entity.CreatedBy != null)
         {
            CreatedBy = _modelObjectFactory.CreateUser();
            CreatedBy.SetModelObjectFactory(_modelObjectFactory);
            await CreatedBy.LoadPropertiesFromEntityAsync(entity.CreatedBy, false);            
         }

         if (entity.LastModifiedBy != null)
         {
            LastModifiedBy = _modelObjectFactory.CreateUser();
            LastModifiedBy.SetModelObjectFactory(_modelObjectFactory);
            await LastModifiedBy.LoadPropertiesFromEntityAsync(entity.LastModifiedBy, false);
         }
      }

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

      public virtual bool CanBeDeleted()
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

      internal List<Infrastructure.DatabaseFields.DatabaseField> ConvertDirtyPropertiesToDataProperties()
      {
         return ConvertDatabaseFieldsToDataProperties(GetDirtyDatabaseFields());
      }

      internal List<Infrastructure.DatabaseFields.DatabaseField> ConvertPropertiesToDataProperties()
      {
         return ConvertDatabaseFieldsToDataProperties(GetDatabaseFields());
      }

      protected Infrastructure.DatabaseFields.DatabaseField ConvertDatabaseFieldToDataProperty(DatabaseField databaseField)
      {
         switch (databaseField)
         {
            case StringDatabaseField stringDatabaseField:
               return new Infrastructure.DatabaseFields.StringDatabaseField(stringDatabaseField.Value, stringDatabaseField.DatabaseFieldName);
            case IntegerDatabaseField integerDatabaseField:
               return new Infrastructure.DatabaseFields.IntegerDatabaseField(integerDatabaseField.Value, integerDatabaseField.DatabaseFieldName);
            case DateTimeDatabaseField dateTimeDatabaseField:
               return new Infrastructure.DatabaseFields.DateTimeDatabaseField(dateTimeDatabaseField.Value, dateTimeDatabaseField.DatabaseFieldName);
            case DateOnlyDatabaseField dateOnlyDatabaseField:
               return new Infrastructure.DatabaseFields.DateOnlyDatabaseField(dateOnlyDatabaseField.Value, dateOnlyDatabaseField.DatabaseFieldName);
            case BoolDatabaseField boolDatabaseField:
               return new Infrastructure.DatabaseFields.BoolDatabaseField(boolDatabaseField.Value, boolDatabaseField.DatabaseFieldName);
            case NullableIntegerDatabaseField nullableIntegerDatabaseField:
               return new Infrastructure.DatabaseFields.NullableIntegerDatabaseField(nullableIntegerDatabaseField.Value, nullableIntegerDatabaseField.DatabaseFieldName);
            default:
               throw new Exception("Unrecognized type " + databaseField.GetType());
         }
      }

      private List<Infrastructure.DatabaseFields.DatabaseField> ConvertDatabaseFieldsToDataProperties(List<DatabaseField> databaseFields)
      {
         var entityDataProperties = new List<Infrastructure.DatabaseFields.DatabaseField>();

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
            return Id.Value == item.Id.Value;
         }
      }

      public virtual void Dispose()
      {
         CreatedBy?.Dispose();
         LastModifiedBy?.Dispose();
      }

      #endregion
   }
}
