using OtherSideCore.Domain.DomainObjects;
using System.Linq.Expressions;

namespace OtherSideCore.Application
{
   public class Constraint<T> where T : DomainObject, new()
   {
      #region Fields



      #endregion

      #region Properties

      

      public Expression<Func<T, bool>> Expression { get; private set; }

      public static Constraint<T> Empty => new Constraint<T>(x => true);

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public Constraint(Expression<Func<T, bool>> expression)
      {
         Expression = expression;
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
