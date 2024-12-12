using OtherSideCore.Domain.DomainObjects;
using System.Linq.Expressions;

namespace OtherSideCore.Application.Browser
{
   public class Constraint<T> where T : DomainObject, new()
   {
      #region Fields



      #endregion

      #region Properties

      public string Name { get; set; }

      public Expression<Func<T, bool>> Expression { get; private set; }

      public static Constraint<T> Empty => new Constraint<T>("", x => true);

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public Constraint(string name, Expression<Func<T, bool>> expression)
      {
         Name = name;
         Expression = expression;
      }

      public Constraint(Expression<Func<T, bool>> expression)
      {
         Name = "Contrainte";
         Expression = expression;
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
