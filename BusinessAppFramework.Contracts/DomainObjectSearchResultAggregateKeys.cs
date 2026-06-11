using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace BusinessAppFramework.Contracts
{
    public static class DomainObjectSearchResultAggregateKeys
    {
        public static string Type(Type t) => KebabStringFormatter.ToKebab(t.Name);
    }

    public static class DomainObjectSearchResultAggregateKeys<T>
    {
        public static string Type => DomainObjectSearchResultAggregateKeys.Type(typeof(T));
    }
}
