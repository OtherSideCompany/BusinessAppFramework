using OtherSideCore.Domain.DatabaseFields;

namespace OtherSideCore.Domain.Tests.DatabaseFields
{
   public class IntegerDatabaseFieldTests
   {
      private IntegerDatabaseField _integerDatabaseField;

      public IntegerDatabaseFieldTests()
      {
         _integerDatabaseField = new IntegerDatabaseField("Test");
      }

      [Fact]
      public void Constructor_DoNotTrigIsDirty()
      {
         Assert.False(_integerDatabaseField.IsDirty);
      }

      [Fact]
      public void LoadValue_DoNotTrigIsDirty()
      {
         _integerDatabaseField.LoadValue(1);

         Assert.False(_integerDatabaseField.IsDirty);
      }

      [Fact]
      public void LoadValue_ValueIsSet()
      {
         _integerDatabaseField.LoadValue(1);

         Assert.Equal(1, _integerDatabaseField.Value);
      }

      [Fact]
      public void SetValue_WorksIfIsEditable()
      {
         Assert.True(_integerDatabaseField.IsEditable);

         _integerDatabaseField.Value = 1;

         Assert.Equal(1, _integerDatabaseField.Value);

         _integerDatabaseField.IsEditable = false;
         _integerDatabaseField.Value = 2;

         Assert.NotEqual(2, _integerDatabaseField.Value);
      }

      [Fact]
      public void SetValue_TrigsIsDirty()
      {
         _integerDatabaseField.Value = 1;

         Assert.True(_integerDatabaseField.IsDirty);
      }

      [Fact]
      public void SetSameValue_DoNotTrigsIsDirty()
      {
         Assert.Equal(0, _integerDatabaseField.Value);

         _integerDatabaseField.Value = 0;

         Assert.False(_integerDatabaseField.IsDirty);
         Assert.Equal(0, _integerDatabaseField.Value);
      }

      [Fact]
      public void LoadValue_BufferIsSet()
      {
         _integerDatabaseField.LoadValue(1);

         Assert.Equal("1", _integerDatabaseField.Buffer);
      }

      [Theory]
      [InlineData("1", 1)]
      [InlineData("-", 0)]
      [InlineData("-12", -12)]
      [InlineData("+4", 4)]
      [InlineData("+", 0)]
      [InlineData("1200 ", 1200)]
      public void SetBuffer_ValueIsSet(string buffer, int value)
      {
         _integerDatabaseField.Buffer = buffer;

         Assert.Equal(value, _integerDatabaseField.Value);
         Assert.Equal(buffer.Trim(), _integerDatabaseField.Buffer);
      }
   }
}
