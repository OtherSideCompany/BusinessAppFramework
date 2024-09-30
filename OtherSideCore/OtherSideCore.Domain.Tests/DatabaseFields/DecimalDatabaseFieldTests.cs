using OtherSideCore.Domain.DatabaseFields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Domain.Tests.DatabaseFields
{
   public class DecimalDatabaseFieldTests
   {
      private DecimalDatabaseField _decimalDatabaseField;

      public DecimalDatabaseFieldTests()
      {
         _decimalDatabaseField = new DecimalDatabaseField("Test");
      }

      [Fact]
      public void Constructor_DoNotTrigIsDirty()
      {
         Assert.False(_decimalDatabaseField.IsDirty);
      }

      [Fact]
      public void LoadValue_DoNotTrigIsDirty()
      {
         _decimalDatabaseField.LoadValue(1.0M);

         Assert.False(_decimalDatabaseField.IsDirty);
      }

      [Fact]
      public void LoadValue_ValueIsSet()
      {
         _decimalDatabaseField.LoadValue(1.0M);

         Assert.Equal(1, _decimalDatabaseField.Value);
      }

      [Fact]
      public void SetValue_WorksIfIsEditable()
      {
         Assert.True(_decimalDatabaseField.IsEditable);

         _decimalDatabaseField.Value = 1.0M;

         Assert.Equal(1.0M, _decimalDatabaseField.Value);

         _decimalDatabaseField.IsEditable = false;
         _decimalDatabaseField.Value = 2.0M;

         Assert.NotEqual(2.0M, _decimalDatabaseField.Value);
      }

      [Fact]
      public void SetValue_TrigsIsDirty()
      {
         _decimalDatabaseField.Value = 1.0M;

         Assert.True(_decimalDatabaseField.IsDirty);
      }

      [Fact]
      public void SetSameValue_DoNotTrigsIsDirty()
      {
         Assert.Equal(0, _decimalDatabaseField.Value);

         _decimalDatabaseField.Value = 0;

         Assert.False(_decimalDatabaseField.IsDirty);
         Assert.Equal(0, _decimalDatabaseField.Value);
      }

      [Fact]
      public void LoadValue_BufferIsSet()
      {
         _decimalDatabaseField.LoadValue(1.0M);

         Assert.Equal("1.00", _decimalDatabaseField.Buffer);
      }

      [Theory]
      [InlineData("1", 1.0)]
      [InlineData(".", 0.0)]
      [InlineData("-.", 0.0)]
      [InlineData("-12.4", -12.4)]
      [InlineData("1.", 1.0)]
      [InlineData(".18", 0.18)]
      [InlineData("+.4", 0.4)]
      [InlineData("+0.4", 0.4)]
      [InlineData("-", 0.0)]
      [InlineData("+", 0.0)]
      [InlineData("-8.", -8.0)]
      [InlineData("-.01", -0.01)]
      [InlineData(".012", 0.012)]
      [InlineData(".916 ", 0.916)]
      public void SetBuffer_ValueIsSet(string buffer, decimal value)
      {
         _decimalDatabaseField.Buffer = buffer;

         Assert.Equal(value, _decimalDatabaseField.Value);
         Assert.Equal(buffer.Trim(), _decimalDatabaseField.Buffer);
      }
   }
}
