using OtherSideCore.Domain.DatabaseFields;

namespace OtherSideCore.Domain.ModelObjects
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
