using OtherSideCore.Application.Search;
using OtherSideCore.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace OtherSideCore.Application.Interfaces
{
    public interface IConstraintFactory
    {
        void RegisterConstraint<TSearchResult>(StringKey key, Expression<Func<TSearchResult, bool>> constraint) where TSearchResult : DomainObjectSearchResult;
        Expression<Func<TSearchResult, bool>> GetConstraint<TSearchResult>(StringKey key) where TSearchResult : DomainObjectSearchResult;
    }
}
