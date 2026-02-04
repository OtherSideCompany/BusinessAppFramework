using Application.Search;
using Domain;
using System.Linq.Expressions;

namespace Application.Interfaces
{
   public interface IConstraintFactory
   {
      void RegisterConstraint<TSearchResult>(StringKey key, Expression<Func<TSearchResult, bool>> constraint) where TSearchResult : DomainObjectSearchResult;
      Expression<Func<TSearchResult, bool>> GetConstraint<TSearchResult>(StringKey key) where TSearchResult : DomainObjectSearchResult;
   }
}
