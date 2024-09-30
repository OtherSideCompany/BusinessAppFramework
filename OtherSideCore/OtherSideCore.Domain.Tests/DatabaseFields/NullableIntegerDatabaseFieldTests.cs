using OtherSideCore.Domain.DatabaseFields;

namespace OtherSideCore.Domain.Tests.DatabaseFields
{
   public class NullableIntegerDatabaseFieldTests
   {
      private NullableIntegerDatabaseField _nullableIntegerDatabaseField;

      public NullableIntegerDatabaseFieldTests()
      {
         _nullableIntegerDatabaseField = new NullableIntegerDatabaseField("Test");
      }

      [Fact]
      public void Constructor_DoNotTrigIsDirty()
      {
         Assert.False(_nullableIntegerDatabaseField.IsDirty);
      }

      [Fact]
      public void LoadValue_DoNotTrigIsDirty()
      {
         _nullableIntegerDatabaseField.LoadValue(1);

         Assert.False(_nullableIntegerDatabaseField.IsDirty);
      }

      [Fact]
      public void LoadValue_ValueIsSet()
      {
         _nullableIntegerDatabaseField.LoadValue(1);

         Assert.Equal(1, _nullableIntegerDatabaseField.Value);
      }

      [Fact]
      public void LoadValue_NullValueIsSet()
      {
         _nullableIntegerDatabaseField.LoadValue(null);

         Assert.Null(_nullableIntegerDatabaseField.Value);
      }

      [Fact]
      public void SetValue_WorksIfIsEditable()
      {
         Assert.True(_nullableIntegerDatabaseField.IsEditable);

         _nullableIntegerDatabaseField.Value = 1;

         Assert.Equal(1, _nullableIntegerDatabaseField.Value);

         _nullableIntegerDatabaseField.IsEditable = false;
         _nullableIntegerDatabaseField.Value = 2;

         Assert.NotEqual(2, _nullableIntegerDatabaseField.Value);
      }

      [Fact]
      public void SetValue_TrigsIsDirty()
      {
         _nullableIntegerDatabaseField.Value = 1;

         Assert.True(_nullableIntegerDatabaseField.IsDirty);
      }

      [Fact]
      public void SetValue_NullTrigsIsDirty()
      {
         _nullableIntegerDatabaseField.LoadValue(1);
         _nullableIntegerDatabaseField.Value = null;

         Assert.True(_nullableIntegerDatabaseField.IsDirty);
      }

      [Fact]
      public void SetSameValue_DoNotTrigsIsDirty()
      {
         _nullableIntegerDatabaseField.LoadValue(0);
         Assert.Equal(0, _nullableIntegerDatabaseField.Value);

         _nullableIntegerDatabaseField.Value = 0;

         Assert.False(_nullableIntegerDatabaseField.IsDirty);
         Assert.Equal(0, _nullableIntegerDatabaseField.Value);
      }

      [Fact]
      public void LoadValue_BufferIsSet()
      {
         _nullableIntegerDatabaseField.LoadValue(1);

         Assert.Equal("1", _nullableIntegerDatabaseField.Buffer);
      }

      [Theory]
      [InlineData("1", 1)]
      [InlineData("-", 0)]
      [InlineData("-12", -12)]
      [InlineData("+4", 4)]
      [InlineData("+", 0)]
      [InlineData("1200 ", 1200)]
      [InlineData("   ", null)]
      [InlineData("", null)]
      public void SetBuffer_ValueIsSet(string buffer, int? value)
      {
         _nullableIntegerDatabaseField.Buffer = buffer;

         Assert.Equal(value, _nullableIntegerDatabaseField.Value);
         Assert.Equal(buffer.Trim(), _nullableIntegerDatabaseField.Buffer);
      }
   }
}
