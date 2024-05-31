using OtherSideCore.Model.DatabaseFields;
using OtherSideCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Model
{
   public abstract class User : ModelObjectBase
   {
      #region Fields

      private StringDatabaseField m_FirstName;
      private StringDatabaseField m_LastName;
      private StringDatabaseField m_UserName;

      #endregion

      #region Properties

      public StringDatabaseField FirstName
      {
         get
         {
            return m_FirstName;
         }
         set
         {
            if (value != m_FirstName)
            {
               m_FirstName = value;
               OnPropertyChanged(nameof(FirstName));
            }
         }
      }

      public StringDatabaseField LastName
      {
         get
         {
            return m_LastName;
         }
         set
         {
            if (value != m_LastName)
            {
               m_LastName = value;
               OnPropertyChanged(nameof(LastName));
            }
         }
      }

      public StringDatabaseField UserName
      {
         get
         {
            return m_UserName;
         }
         set
         {
            if (value != m_UserName)
            {
               m_UserName = value;
               OnPropertyChanged(nameof(UserName));
            }
         }
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public User() : base()
      {
         Id = new IntegerDatabaseField("UserId");
         FirstName = new StringDatabaseField("FirstName", 50);
         LastName = new StringDatabaseField("LastName", 50);
         UserName = new StringDatabaseField("UserName", 50);
      }

      #endregion

      #region Methods

      public abstract bool Authenticate(string passwordHash);

      #endregion
   }
}
