using OtherSideCore.Model.DatabaseFields;
using OtherSideCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OtherSideCore.Model
{
   public abstract class ModelObjectBase : ObservableObject, IDisposable
   {
      #region Fields

      private IntegerDatabaseField m_Id;
      private bool m_IsLoading;
      private bool m_IsDirty;

      #endregion

      #region Properties

      public Guid guid { get; set; }

      public IntegerDatabaseField Id
      {
         get
         {
            return m_Id;
         }
         set
         {
            if (value != m_Id)
            {
               m_Id = value;
               OnPropertyChanged(nameof(Id));
               OnPropertyChanged(nameof(IsCreated));
            }
         }
      }

      public bool IsLoading
      {
         get
         {
            return m_IsLoading;
         }
         set
         {
            if (value != m_IsLoading)
            {
               m_IsLoading = value;
               OnPropertyChanged(nameof(IsLoading));
            }
         }
      }

      public bool IsCreated
      {
         get
         {
            return Id.Value != 0;
         }
      }

      public bool IsDirty
      {
         get
         {
            return m_IsDirty;
         }
         set
         {
            if (value != m_IsDirty)
            {
               m_IsDirty = value;
               OnPropertyChanged(nameof(IsDirty));
            }
         }
      }

      #endregion

      #region Constructor

      public ModelObjectBase()
      {
         guid = Guid.NewGuid();
      }

      #endregion

      #region Virtual Methods

      public virtual bool MatchFilter(List<string> filters, bool extendedSearch)
      {
         return false;
      }

      public abstract void Load();

      public abstract bool CanSaveChanges();

      public abstract bool CanCancelChanges();

      public abstract void Save();

      public abstract bool CanBeDeleted();

      public virtual void Delete()
      {
         IsLoading = true;
         Id.Value = 0;
         IsLoading = false;
      }

      public abstract void Dispose();

      #endregion

      #region Methods

      protected List<Data.DatabaseFields.DatabaseField> ConvertDirtyPropertiesToDataProperties()
      {
         var databaseProperties = typeof(ModelObjectBase).GetProperties().Where(p => p.PropertyType == typeof(DatabaseField))
                                                                         .Select(p => p.GetValue(this))
                                                                         .Where(p => (p as DatabaseField).IsDirty)
                                                                         .Cast<DatabaseField>()
                                                                         .ToList();

         return ConvertDatabaseFieldsToDataProperties(databaseProperties);
      }

      protected List<Data.DatabaseFields.DatabaseField> ConvertPropertiesToDataProperties()
      {
         var databaseProperties = typeof(ModelObjectBase).GetProperties().Where(p => p.PropertyType == typeof(DatabaseField))
                                                                         .Select(p => p.GetValue(this))
                                                                         .Cast<DatabaseField>()
                                                                         .ToList();

         return ConvertDatabaseFieldsToDataProperties(databaseProperties);
      }

      private List<Data.DatabaseFields.DatabaseField> ConvertDatabaseFieldsToDataProperties(List<DatabaseField> databaseFields)
      {
         var entityDataProperties = new List<OtherSideCore.Data.DatabaseFields.DatabaseField>();

         foreach (var databaseField in databaseFields)
         {
            switch (databaseField)
            {
               case StringDatabaseField stringDatabaseField:
                  entityDataProperties.Add(new Data.DatabaseFields.StringDatabaseField(stringDatabaseField.Value, stringDatabaseField.DatabaseFieldName));
                  break;
               case IntegerDatabaseField integerDatabaseField:
                  entityDataProperties.Add(new Data.DatabaseFields.IntegerDatabaseField(integerDatabaseField.Value, integerDatabaseField.DatabaseFieldName));
                  break;
               default:
                  throw new Exception("Unrecognized type " + databaseField.GetType());
                  break;
            }
         }

         return entityDataProperties;
      }

      public override bool Equals(object obj)
      {
         var item = obj as ModelObjectBase;

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

      #endregion
   }
}
