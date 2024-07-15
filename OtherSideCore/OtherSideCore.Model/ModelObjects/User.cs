using OtherSideCore.Model.DatabaseFields;
using OtherSideCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Model.ModelObjects
{
   public abstract class User : ModelObject
   {
      #region Fields

      private const int SUPERADMINID = 1;

      private BoolDatabaseField m_IsSuperAdmin;
      private StringDatabaseField m_FirstName;
      private StringDatabaseField m_LastName;
      private StringDatabaseField m_UserName;

      #endregion

      #region Properties

      public BoolDatabaseField IsSuperAdmin
      {
         get => m_IsSuperAdmin;
         private set => SetProperty(ref m_IsSuperAdmin, value);
      }

      public StringDatabaseField FirstName
      {
         get => m_FirstName;
         private set => SetProperty(ref m_FirstName, value);
      }

      public StringDatabaseField LastName
      {
         get => m_LastName;
         private set => SetProperty(ref m_LastName, value);
      }

      public StringDatabaseField UserName
      {
         get => m_UserName;
         private set => SetProperty(ref m_UserName, value);
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public User() : base()
      {
         IsSuperAdmin = new BoolDatabaseField("IsSuperAdmin");
         FirstName = new StringDatabaseField("FirstName", 50);
         LastName = new StringDatabaseField("LastName", 50);
         UserName = new StringDatabaseField("UserName", 50);
      }

      #endregion

      #region Methods

      public abstract Task<bool> Authenticate(string passwordHash);

      #endregion
   }
}
