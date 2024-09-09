using OtherSideCore.Domain.DatabaseFields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Domain.Tests.DatabaseFields
{
   public class DateOnlyDatabaseFieldTests
   {
      private DateOnlyDatabaseField _dateOnlyDatabaseField;

      public DateOnlyDatabaseFieldTests()
      {
         _dateOnlyDatabaseField = new DateOnlyDatabaseField("Test");
         _dateOnlyDatabaseField.LoadValue(DateOnly.FromDateTime(DateTime.Now.AddDays(-1)));
      }

      [Fact]
      public void Constructor_DoNotTrigIsDirty()
      {
         Assert.False(_dateOnlyDatabaseField.IsDirty);
      }

      [Fact]
      public void LoadValue_DoNotTrigIsDirty()
      {
         var now = DateOnly.FromDateTime(DateTime.Now);

         _dateOnlyDatabaseField.LoadValue(now);

         Assert.False(_dateOnlyDatabaseField.IsDirty);
      }

      [Fact]
      public void LoadValue_ValueIsSet()
      {
         var now = DateOnly.FromDateTime(DateTime.Now);

         _dateOnlyDatabaseField.LoadValue(now);

         Assert.Equal(now, _dateOnlyDatabaseField.Value);
      }

      [Fact]
      public void SetValue_WorksIfIsEditable()
      {
         var now = DateOnly.FromDateTime(DateTime.Now);

         Assert.True(_dateOnlyDatabaseField.IsEditable);

         _dateOnlyDatabaseField.Value = now;

         Assert.Equal(now, _dateOnlyDatabaseField.Value);

         var later = DateOnly.FromDateTime(DateTime.Now.AddDays(1));

         _dateOnlyDatabaseField.IsEditable = false;
         _dateOnlyDatabaseField.Value = later;

         Assert.NotEqual(later, _dateOnlyDatabaseField.Value);
      }

      [Fact]
      public void SetValue_TrigsIsDirty()
      {
         var now = DateOnly.FromDateTime(DateTime.Now);

         _dateOnlyDatabaseField.Value = now;

         Assert.True(_dateOnlyDatabaseField.IsDirty);
      }

      [Fact]
      public void SetSameValue_DoNotTrigsIsDirty()
      {
         _dateOnlyDatabaseField.Value = _dateOnlyDatabaseField.Value;

         Assert.False(_dateOnlyDatabaseField.IsDirty);
      }
   }
}
