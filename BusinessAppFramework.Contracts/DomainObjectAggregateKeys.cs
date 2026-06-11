using System.Linq.Expressions;
using System.Reflection;

namespace BusinessAppFramework.Contracts
{
    public static class DomainObjectAggregateKeys
    {
        public static string Type(Type t) => KebabStringFormatter.ToKebab(t.Name);
        public static string Workspace(Type t) => KebabStringFormatter.ToKebab(t.Name) + "-workspace";
        public static string PageWorkspace(Type t) => KebabStringFormatter.ToKebab(t.Name) + "-page";
        public static string PageTree(Type t) => KebabStringFormatter.ToKebab(t.Name) + "-page-tree";
        public static string PermissionKey(Type t) => KebabStringFormatter.ToKebab(t.Name) + "-permission-key";

        public static string Property(Type t, string memberName)
            => $"{KebabStringFormatter.ToKebab(t.Name)}-{KebabStringFormatter.ToKebab(memberName)}";
    }

    public static class DomainObjectAggregateKeys<T>
    {
        public static string Type => DomainObjectAggregateKeys.Type(typeof(T));
        public static string Workspace => DomainObjectAggregateKeys.Workspace(typeof(T));
        public static string PageWorkspace => DomainObjectAggregateKeys.PageWorkspace(typeof(T));
        public static string PageTree => DomainObjectAggregateKeys.PageTree(typeof(T));
        public static string PermissionKey => DomainObjectAggregateKeys.PermissionKey(typeof(T));

        public static string Property(Expression<Func<T, object?>> prop)
        {
            var member = GetMember(prop);
            return DomainObjectAggregateKeys.Property(typeof(T), member.Name);
        }

        static MemberInfo GetMember(Expression<Func<T, object?>> expr)
        {
            var body = expr.Body is UnaryExpression u ? u.Operand : expr.Body;
            if (body is MemberExpression m) return m.Member;
            throw new ArgumentException("Expression must target property.", nameof(expr));
        }
    }
}
