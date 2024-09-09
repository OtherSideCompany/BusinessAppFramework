using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Domain.DatabaseFields
{
   public class DateOnlyDatabaseField : DatabaseField
   {
      #region Fields

      private DateOnly m_Value;
      private string m_Buffer;

      #endregion

      #region Properties

      public DateOnly Value
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

      public DateOnlyDatabaseField(string databaseFieldName) : base(databaseFieldName)
      {
         m_IsLoading = true;

         Value = DateOnly.FromDateTime(DateTime.Now);

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

         Value = (DateOnly)value;

         m_IsLoading = false;
      }

      #endregion
   }
}
