using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Domain.DatabaseFields
{
   public class BoolDatabaseField : DatabaseField
   {
      #region Fields

      private bool m_Value;

      #endregion

      #region Properties

      public bool Value
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

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public BoolDatabaseField(string databaseFieldName) : base(databaseFieldName)
      {
         m_IsLoading = true;

         Value = false;

         m_IsLoading = false;
      }

      #endregion

      #region Methods

      public void LoadBuffer()
      {

      }

      public void UpdateBuffer()
      {

      }

      public override void LoadValue(object value)
      {
         m_IsLoading = true;

         Value = (bool)value;

         m_IsLoading = false;
      }

      #endregion
   }
}
