using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Search;
using BusinessAppFramework.Domain;
using System.Linq.Expressions;

namespace BusinessAppFramework.Application.Factories
{
   public class ConstraintFactory : StringKeyBasedFactory, IConstraintFactory
   {
      #region Fields



      #endregion

      #region Properties



      #endregion

      #region Events



      #endregion

      #region Constructor

      public ConstraintFactory()
      {

      }

      #endregion

      #region Public Methods

      public void RegisterConstraint<TSearchResult>(StringKey key, Expression<Func<TSearchResult, bool>> constraint) where TSearchResult : DomainObjectSearchResult
      {
         Register(key, searchResult => constraint);
      }

      public Expression<Func<TSearchResult, bool>> GetConstraint<TSearchResult>(StringKey key) where TSearchResult : DomainObjectSearchResult
      {
         return (Expression<Func<TSearchResult, bool>>)Create(key);
      }

      #endregion

      #region Private Methods


      #endregion
   }
}
