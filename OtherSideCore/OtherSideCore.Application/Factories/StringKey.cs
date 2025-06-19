using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Application
{
   public class StringKey : IEquatable<StringKey>
   {
      #region Fields



      #endregion

      #region Properties

      public string Key { get; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      private StringKey(string key) => Key = key;

      #endregion

      #region Public Methods

      public static StringKey From(string key) => new(key);
      public override string ToString() => Key;
      public override bool Equals(object? obj) => obj is StringKey other && Equals(other);
      public bool Equals(StringKey? other) => other is not null && string.Equals(Key, other.Key, StringComparison.OrdinalIgnoreCase);
      public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Key);

      #endregion

      #region Private Methods



      #endregion
   }
}
