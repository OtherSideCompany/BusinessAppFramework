using OtherSideCore.Model.DatabaseFields;
using OtherSideCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Model
{
   public abstract class User : ModelObject
   {
      #region Fields

      private StringDatabaseField m_FirstName;
      private StringDatabaseField m_LastName;
      private StringDatabaseField m_UserName;

      #endregion

      #region Properties

      public StringDatabaseField FirstName
      {
         get => m_FirstName;
         set => SetProperty(ref m_FirstName, value);
      }

      public StringDatabaseField LastName
      {
         get => m_LastName;
         set => SetProperty(ref m_LastName, value);
      }

      public StringDatabaseField UserName
      {
         get => m_UserName;
         set => SetProperty(ref m_UserName, value);
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public User() : base()
      {
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
