using OtherSideCore.Infrastructure.DatabaseFields;
using OtherSideCore.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Infrastructure.Repositories
{
   public class Constraint<T> where T : EntityBase
   {
      #region Fields



      #endregion

      #region Properties

      public string DatabaseFieldName { get; private set; }
      public object Value { get; private set; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public Constraint(string databaseFieldName, object constraintValue)
      {
         DatabaseFieldName = databaseFieldName;
         Value = constraintValue;
      }

      #endregion

      #region Public Methods

      public Expression<Func<T, bool>> LambdaConstructor()
      {
         var item = Expression.Parameter(typeof(T), "item");
         var prop = Expression.Property(item, DatabaseFieldName);
         var propertyInfo = typeof(T).GetProperty(DatabaseFieldName);
         var value = Expression.Constant(Convert.ChangeType(Value.ToString(), propertyInfo.PropertyType));

         BinaryExpression equal = Expression.Equal(prop, value);
         //switch (condition)
         //{
         //   case Condition.eq:
         //      equal = Expression.Equal(prop, value);
         //      break;
         //   case Condition.gt:
         //      equal = Expression.GreaterThan(prop, value);
         //      break;
         //   case Condition.gte:
         //      equal = Expression.GreaterThanOrEqual(prop, value);
         //      break;
         //   case Condition.lt:
         //      equal = Expression.LessThan(prop, value);
         //      break;
         //   case Condition.lte:
         //      equal = Expression.LessThanOrEqual(prop, value);
         //      break;
         //   default:
         //      equal = Expression.Equal(prop, value);
         //      break;
         //}
         var lambda = Expression.Lambda<Func<T, bool>>(equal, item);

         return lambda;
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
