using OtherSideCore.Domain.DatabaseFields;
using OtherSideCore.Domain.ModelObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Domain.Tests.Repositories
{
   public class DefaultModelObject : ModelObject
   {
      private StringDatabaseField _randomProperty;

      public StringDatabaseField RandomProperty
      {
         get => _randomProperty;
         set => SetProperty(ref _randomProperty, value);
      }

      public DefaultModelObject() : base()
      {
         RandomProperty = new StringDatabaseField(nameof(RandomProperty), 50);
      }
   }
}
