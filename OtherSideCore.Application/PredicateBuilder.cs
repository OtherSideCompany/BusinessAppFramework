using System.Linq.Expressions;

namespace OtherSideCore.Application
{
   public static class PredicateBuilder
   {
      public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
      {
         var parameter = Expression.Parameter(typeof(T));

         var combined = Expression.AndAlso(Expression.Invoke(first, parameter), Expression.Invoke(second, parameter));

         return Expression.Lambda<Func<T, bool>>(combined, parameter);
      }

      public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
      {
         var parameter = Expression.Parameter(typeof(T));

         var combined = Expression.OrElse(Expression.Invoke(first, parameter), Expression.Invoke(second, parameter));

         return Expression.Lambda<Func<T, bool>>(combined, parameter);
      }
   }
}
