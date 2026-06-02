using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Domain;
using System.Linq.Expressions;

namespace BusinessAppFramework.Application.Factories
{
    public interface IDomainObjectNavigationApplicationActionFactory
    {
        void Register(StringKey key, Func<IDomainObjectNavigationApplicationAction> constraint);
        IDomainObjectNavigationApplicationAction Get(StringKey key);
    }
}
