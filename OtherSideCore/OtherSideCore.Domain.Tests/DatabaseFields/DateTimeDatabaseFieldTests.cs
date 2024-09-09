using OtherSideCore.Domain.DatabaseFields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Domain.Tests.DatabaseFields
{
   public class DateTimeDatabaseFieldTests
   {
      private DateTimeDatabaseField _dateTimeDatabaseField;

      public DateTimeDatabaseFieldTests()
      {
         _dateTimeDatabaseField = new DateTimeDatabaseField("Test");
         _dateTimeDatabaseField.LoadValue(DateTime.Now.AddDays(-1));
      }

      [Fact]
      public void Constructor_DoNotTrigIsDirty()
      {
         Assert.False(_dateTimeDatabaseField.IsDirty);
      }

      [Fact]
      public void LoadValue_DoNotTrigIsDirty()
      {
         var now = DateTime.Now;

         _dateTimeDatabaseField.LoadValue(now);

         Assert.False(_dateTimeDatabaseField.IsDirty);
      }

      [Fact]
      public void LoadValue_ValueIsSet()
      {
         var now = DateTime.Now;

         _dateTimeDatabaseField.LoadValue(now);

         Assert.Equal(now, _dateTimeDatabaseField.Value);
      }

      [Fact]
      public void SetValue_WorksIfIsEditable()
      {
         var now = DateTime.Now;

         Assert.True(_dateTimeDatabaseField.IsEditable);

         _dateTimeDatabaseField.Value = now;

         Assert.Equal(now, _dateTimeDatabaseField.Value);

         var later = DateTime.Now.AddDays(1);

         _dateTimeDatabaseField.IsEditable = false;
         _dateTimeDatabaseField.Value = later;

         Assert.NotEqual(later, _dateTimeDatabaseField.Value);
      }

      [Fact]
      public void SetValue_TrigsIsDirty()
      {
         var now = DateTime.Now;

         _dateTimeDatabaseField.Value = now;

         Assert.True(_dateTimeDatabaseField.IsDirty);
      }

      [Fact]
      public void SetSameValue_DoNotTrigsIsDirty()
      {
         _dateTimeDatabaseField.Value = _dateTimeDatabaseField.Value;

         Assert.False(_dateTimeDatabaseField.IsDirty);
      }
   }
}
