using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace BusinessAppFramework.Contracts
{
    public static class AggregateKeys
    {
        public static string Workspace(Type t) => KebabStringFormatter.ToKebab(t.Name) + "-workspace";
        public static string PageWorkspace(Type t) => KebabStringFormatter.ToKebab(t.Name) + "-page";
        public static string PageTree(Type t) => KebabStringFormatter.ToKebab(t.Name) + "-page-tree";
        public static string PermissionKey(Type t) => KebabStringFormatter.ToKebab(t.Name) + "-permission-key";

        public static string Property(Type t, string memberName)
            => $"{KebabStringFormatter.ToKebab(t.Name)}-{KebabStringFormatter.ToKebab(memberName)}";
    }

    public static class AggregateKeys<T>
    {
        public static string Workspace => AggregateKeys.Workspace(typeof(T));
        public static string PageWorkspace => AggregateKeys.PageWorkspace(typeof(T));
        public static string PageTree => AggregateKeys.PageTree(typeof(T));
        public static string PermissionKey => AggregateKeys.PermissionKey(typeof(T));

        public static string Property(Expression<Func<T, object?>> prop)
        {
            var member = GetMember(prop);
            return AggregateKeys.Property(typeof(T), member.Name);
        }

        static MemberInfo GetMember(Expression<Func<T, object?>> expr)
        {
            var body = expr.Body is UnaryExpression u ? u.Operand : expr.Body;
            if (body is MemberExpression m) return m.Member;
            throw new ArgumentException("Expression must target property.", nameof(expr));
        }
    }
}
