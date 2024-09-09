using System;

namespace OtherSideCore.Domain.DatabaseFields
{
   public class DateTimeDatabaseField : DatabaseField
   {
      #region Fields

      private DateTime m_Value;
      private string m_Buffer;

      #endregion

      #region Properties

      public DateTime Value
      {
         get => m_Value;
         set
         {
            var updateDirtySate = !m_IsLoading && value != m_Value;

            if (IsEditable)
            {
               SetProperty(ref m_Value, value);

               if (updateDirtySate) IsDirty = true;
            }
         }
      }

      public string Buffer
      {
         get => m_Buffer;
         set => SetProperty(ref m_Buffer, value);
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DateTimeDatabaseField(string databaseFieldName) : base(databaseFieldName)
      {
         m_IsLoading = true;

         Value = DateTime.Now;

         m_IsLoading = false;
      }

      #endregion

      #region Public Methods

      public void LoadBuffer()
      {

      }

      public void UpdateBuffer()
      {

      }

      public override void LoadValue(object value)
      {
         m_IsLoading = true;

         Value = (DateTime)value;

         m_IsLoading = false;
      }

      #endregion
   }
}
