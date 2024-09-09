namespace OtherSideCore.Domain.DatabaseFields
{
   public class StringDatabaseField : DatabaseField
   {
      #region Fields

      private string m_Value;
      private string m_Buffer;
      private int m_MaxLength;

      #endregion

      #region Properties

      public string Value
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

      public int MaxLength
      {
         get => m_MaxLength;
         set => SetProperty(ref m_MaxLength, value);
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public StringDatabaseField(string databaseFieldName, int maxLength) : base(databaseFieldName)
      {
         m_IsLoading = true;

         MaxLength = maxLength;
         Value = GlobalVariables.DefaultString;

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

      public override bool IsValid()
      {
         return Value.Length < MaxLength;
      }

      public override void LoadValue(object value)
      {
         m_IsLoading = true;

         Value = (string)value;

         m_IsLoading = false;
      }

      #endregion
   }
}
