using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Domain.DatabaseFields
{
   public class IntegerDatabaseField : DatabaseField
   {
      #region Fields

      private int _value;

      #endregion

      #region Properties

      public int Value
      {
         get => _value;
         set
         {
            var updateDirtySate = !m_IsLoading && value != _value;

            if (IsEditable)
            {
               SetProperty(ref _value, value);

               if (updateDirtySate) IsDirty = true;
            }
         }
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public IntegerDatabaseField(string databaseFieldName) : base(databaseFieldName)
      {

      }

      #endregion

      #region Public Methods

      public override void LoadValue(object value)
      {
         m_IsLoading = true;

         Value = (int)value;

         m_IsLoading = false;
      }

      #endregion
   }
}
