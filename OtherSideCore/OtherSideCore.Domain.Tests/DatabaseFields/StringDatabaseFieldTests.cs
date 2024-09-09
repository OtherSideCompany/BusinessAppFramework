using OtherSideCore.Domain.DatabaseFields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Domain.Tests.DatabaseFields
{
   public class StringDatabaseFieldTests
   {
      private StringDatabaseField _stringDatabaseField;

      public StringDatabaseFieldTests()
      {
         _stringDatabaseField = new StringDatabaseField("Test", 30);
         _stringDatabaseField.LoadValue("this is a test string");
      }

      [Fact]
      public void Constructor_DoNotTrigIsDirty()
      {
         Assert.False(_stringDatabaseField.IsDirty);
      }

      [Fact]
      public void LoadValue_DoNotTrigIsDirty()
      {
         _stringDatabaseField.LoadValue("string is changed");

         Assert.False(_stringDatabaseField.IsDirty);
      }

      [Fact]
      public void LoadValue_ValueIsSet()
      {
         _stringDatabaseField.LoadValue("string is changed");

         Assert.Equal("string is changed", _stringDatabaseField.Value);
      }

      [Fact]
      public void SetValue_WorksIfIsEditable()
      {
         Assert.True(_stringDatabaseField.IsEditable);

         _stringDatabaseField.Value = "string is changed";

         Assert.Equal("string is changed", _stringDatabaseField.Value);

         _stringDatabaseField.IsEditable = false;
         _stringDatabaseField.Value = "string is changed again";

         Assert.NotEqual("string is changed again", _stringDatabaseField.Value);
      }

      [Fact]
      public void SetValue_TrigsIsDirty()
      {
         _stringDatabaseField.Value = "string is changed";

         Assert.True(_stringDatabaseField.IsDirty);
      }

      [Fact]
      public void SetSameValue_DoNotTrigsIsDirty()
      {
         Assert.Equal("this is a test string", _stringDatabaseField.Value);

         _stringDatabaseField.Value = "this is a test string";

         Assert.False(_stringDatabaseField.IsDirty);
         Assert.Equal("this is a test string", _stringDatabaseField.Value);
      }

      [Fact]
      public void SetTooLongValue_IsNotValid()
      {
         _stringDatabaseField.Value = "this is a test string that is wayyyyyyy too long";

         Assert.False(_stringDatabaseField.IsValid());
      }
   }
}
