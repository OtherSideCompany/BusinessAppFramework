using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace BusinessAppFramework.Contracts
{
    public static class AggregateKeys
    {
        public static string Workspace(Type t) => ToKebab(t.Name) + "-workspace";
        public static string PageWorkspace(Type t) => ToKebab(t.Name) + "-page";
        public static string PageTree(Type t) => ToKebab(t.Name) + "-page-tree";
        public static string PermissionKey(Type t) => ToKebab(t.Name) + "-permission-key";

        public static string Property(Type t, string memberName)
            => $"{ToKebab(t.Name)}-{ToKebab(memberName)}";

        static string ToKebab(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            var sb = new StringBuilder(s.Length + 5);
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                bool needsDash = char.IsUpper(c) && i > 0 &&
                    (!char.IsUpper(s[i - 1]) ||
                     (i + 1 < s.Length && char.IsLower(s[i + 1])));
                if (needsDash) sb.Append('-');
                sb.Append(char.ToLowerInvariant(c));
            }
            return sb.ToString();
        }
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
            return AggregateKeys.Property(member.DeclaringType!, member.Name);
        }

        static MemberInfo GetMember(Expression<Func<T, object?>> expr)
        {
            var body = expr.Body is UnaryExpression u ? u.Operand : expr.Body;
            if (body is MemberExpression m) return m.Member;
            throw new ArgumentException("Expression must target property.", nameof(expr));
        }
    }
}
