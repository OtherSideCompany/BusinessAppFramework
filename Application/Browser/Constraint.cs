using Application.Search;
using System.Linq.Expressions;

namespace Application.Browser
{
   public class Constraint<TSearchResult> where TSearchResult : DomainObjectSearchResult, new()
   {
      #region Fields



      #endregion

      #region Properties

      public string Name { get; set; }

      public Expression<Func<TSearchResult, bool>> Expression { get; private set; }

      public static Constraint<TSearchResult> Empty => new Constraint<TSearchResult>("", x => true);

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public Constraint(string name, Expression<Func<TSearchResult, bool>> expression)
      {
         Name = name;
         Expression = expression;
      }

      public Constraint(Expression<Func<TSearchResult, bool>> expression)
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
