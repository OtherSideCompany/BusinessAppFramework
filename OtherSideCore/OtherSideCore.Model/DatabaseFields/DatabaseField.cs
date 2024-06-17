using OtherSideCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Model.DatabaseFields
{
   public abstract class DatabaseField : ObservableObject
   {
      #region Fields

      private bool m_IsIdentity;
      private string m_DatabaseFieldName;
      private bool m_IsDirty;
      protected bool m_IsLoading;

      #endregion

      #region Properties

      public bool IsIdentity
      {
         get
         {
            return m_IsIdentity;
         }
         set
         {
            if (value != m_IsIdentity)
            {
               m_IsIdentity = value;
               OnPropertyChanged(nameof(IsIdentity));
            }
         }
      }

      public string DatabaseFieldName
      {
         get
         {
            return m_DatabaseFieldName;
         }
         set
         {
            if (value != m_DatabaseFieldName)
            {
               m_DatabaseFieldName = value;
               OnPropertyChanged(nameof(DatabaseFieldName));
            }
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

      #region Commands



      #endregion

      #region Constructor

      public DatabaseField(string databaseFieldName)
      {
         DatabaseFieldName = databaseFieldName;
      }

      #endregion

      #region Methods

      public virtual bool IsValid()
      {
         return true;
      }

      public abstract void LoadValue(object value);

      #endregion
   }
}
