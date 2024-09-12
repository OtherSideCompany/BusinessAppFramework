using OtherSideCore.Domain.DatabaseFields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
   }
}
