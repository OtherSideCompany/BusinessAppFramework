using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Domain
{
    public readonly record struct StringKey : IEquatable<StringKey>
    {
        #region Fields



        #endregion

        #region Properties

        public static StringKey Empty => StringKey.From(string.Empty);

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
        public bool Equals(StringKey other) 
        { 
            return string.Equals(Key, other.Key, StringComparison.OrdinalIgnoreCase); 
        }
        public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Key);

        #endregion

        #region Private Methods



        #endregion
    }
}
