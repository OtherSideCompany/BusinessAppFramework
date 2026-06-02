using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Domain;
using System.Linq.Expressions;

namespace BusinessAppFramework.Application.Factories
{
    public class DomainObjectNavigationApplicationActionFactory : StringKeyBasedFactory, IDomainObjectNavigationApplicationActionFactory
    {
        #region Fields



        #endregion

        #region Properties



        #endregion

        #region Events



        #endregion

        #region Constructor

        public DomainObjectNavigationApplicationActionFactory()
        {

        }

        #endregion

        #region Public Methods

        public IDomainObjectNavigationApplicationAction Get(StringKey key)
        {
            return (IDomainObjectNavigationApplicationAction)Create(key);
        }

        public void Register(StringKey key, Func<IDomainObjectNavigationApplicationAction> navigationApplicationAction)
        {
            base.Register(key, navigationApplicationAction);
        }


        #endregion

        #region Private Methods


        #endregion
    }
}
