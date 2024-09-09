using OtherSideCore.Domain.DatabaseFields;

namespace OtherSideCore.Domain.Tests.DatabaseFields
{
   public class BoolDatabaseFieldTests
   {
      private BoolDatabaseField _boolDatabaseField;

      public BoolDatabaseFieldTests()
      {
         _boolDatabaseField = new BoolDatabaseField("Test");
      }

      [Fact]
      public void Constructor_DoNotTrigIsDirty()
      {
         Assert.False(_boolDatabaseField.IsDirty);
      }

      [Fact]
      public void LoadValue_DoNotTrigIsDirty()
      {
         _boolDatabaseField.LoadValue(true);

         Assert.False(_boolDatabaseField.IsDirty);
      }

      [Fact]
      public void LoadValue_ValueIsSet()
      {
         _boolDatabaseField.LoadValue(true);

         Assert.True(_boolDatabaseField.Value);
      }

      [Fact]
      public void SetValue_WorksIfIsEditable()
      {
         Assert.True(_boolDatabaseField.IsEditable);

         _boolDatabaseField.Value = true;

         Assert.True(_boolDatabaseField.Value);

         _boolDatabaseField.IsEditable = false;
         _boolDatabaseField.Value = false;

         Assert.True(_boolDatabaseField.Value);
      }

      [Fact]
      public void SetValue_TrigsIsDirty()
      {
         _boolDatabaseField.Value = true;

         Assert.True(_boolDatabaseField.IsDirty);
      }

      [Fact]
      public void SetSameValue_DoNotTrigsIsDirty()
      {
         Assert.False(_boolDatabaseField.Value);

         _boolDatabaseField.Value = false;

         Assert.False(_boolDatabaseField.IsDirty);
         Assert.False(_boolDatabaseField.Value);
      }
   }
}