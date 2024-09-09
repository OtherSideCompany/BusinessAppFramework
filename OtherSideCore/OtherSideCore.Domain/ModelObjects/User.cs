using OtherSideCore.Domain.DatabaseFields;

namespace OtherSideCore.Domain.ModelObjects
{
   public class User : ModelObject
   {
      #region Fields

      private BoolDatabaseField _isActive;
      private StringDatabaseField _firstName;
      private StringDatabaseField _lastName;
      private StringDatabaseField _userName;
      private StringDatabaseField _passwordHash;

      #endregion

      #region Properties

      public BoolDatabaseField IsActive
      {
         get => _isActive;
         private set => SetProperty(ref _isActive, value);
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

      public StringDatabaseField PasswordHash
      {
         get => _passwordHash;
         private set => SetProperty(ref _passwordHash, value);
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public User() : base()
      {
         IsActive = new BoolDatabaseField(nameof(IsActive));
         FirstName = new StringDatabaseField(nameof(FirstName), 50);
         LastName = new StringDatabaseField(nameof(LastName), 50);
         UserName = new StringDatabaseField(nameof(UserName), 50);
         PasswordHash = new StringDatabaseField(nameof(PasswordHash), 512);

         IsActive.Value = true;
      }

      #endregion

      #region Public Methods

      public override bool CanBeDeleted()
      {
         return false;
      }

      #endregion
   }
}
