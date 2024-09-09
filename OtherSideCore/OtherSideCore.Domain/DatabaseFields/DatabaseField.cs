using CommunityToolkit.Mvvm.ComponentModel;

namespace OtherSideCore.Domain.DatabaseFields
{
   public abstract class DatabaseField : ObservableObject
   {
      #region Fields

      private string m_DatabaseFieldName;
      private bool m_IsDirty;
      protected bool m_IsLoading;
      private bool m_IsEditable;

      #endregion

      #region Properties

      public string DatabaseFieldName
      {
         get => m_DatabaseFieldName;
         set => SetProperty(ref m_DatabaseFieldName, value);
      }

      public bool IsDirty
      {
         get => m_IsDirty;
         set => SetProperty(ref m_IsDirty, value);
      }

      public bool IsEditable
      {
         get => m_IsEditable;
         set => SetProperty(ref m_IsEditable, value);
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DatabaseField(string databaseFieldName)
      {
         IsEditable = true;
         DatabaseFieldName = databaseFieldName;
      }

      #endregion

      #region Public Methods

      public virtual bool IsValid()
      {
         return true;
      }

      public abstract void LoadValue(object value);

      #endregion
   }
}
