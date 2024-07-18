using OtherSideCore.Data.Entities;
using OtherSideCore.Data;
using OtherSideCore.Model.DatabaseFields;
using OtherSideCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace OtherSideCore.Model.ModelObjects
{
   public class User : ModelObject
   {
      #region Fields

      private const int SUPERADMINID = 1;

      private BoolDatabaseField _isSuperAdmin;
      private StringDatabaseField _firstName;
      private StringDatabaseField _lastName;
      private StringDatabaseField _userName;

      #endregion

      #region Properties

      public BoolDatabaseField IsSuperAdmin
      {
         get => _isSuperAdmin;
         private set => SetProperty(ref _isSuperAdmin, value);
      }

      public StringDatabaseField FirstName
      {
         get => _firstName;
         private set => SetProperty(ref _firstName, value);
      }

      public StringDatabaseField LastName
      {
         get => _lastName;
         private set => SetProperty(ref _lastName, value);
      }

      public StringDatabaseField UserName
      {
         get => _userName;
         private set => SetProperty(ref _userName, value);
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



      #endregion
   }
}
