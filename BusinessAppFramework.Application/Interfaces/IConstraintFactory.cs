using BusinessAppFramework.Application.Search;
using BusinessAppFramework.Domain;
using System.Linq.Expressions;

namespace BusinessAppFramework.Application.Interfaces
{
   public interface IConstraintFactory
   {
      void RegisterConstraint<TSearchResult>(string key, Expression<Func<TSearchResult, bool>> constraint) where TSearchResult : DomainObjectSearchResult;
      Expression<Func<TSearchResult, bool>> GetConstraint<TSearchResult>(string key) where TSearchResult : DomainObjectSearchResult;
   }
}
