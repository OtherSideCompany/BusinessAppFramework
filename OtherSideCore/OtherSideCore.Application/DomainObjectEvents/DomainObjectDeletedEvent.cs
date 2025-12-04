using OtherSideCore.Domain.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Application.DomainObjectEvents
{
    public class DomainObjectDeletedEvent : DomainObjectEvent
    {
        #region Fields



        #endregion

        #region Properties



        #endregion

        #region Commands



        #endregion

        #region Constructor

        public DomainObjectDeletedEvent(Type domainObjectType, int domainObjectId) : base(domainObjectType, domainObjectId)
        {

        }

        #endregion

        #region Public Methods



        #endregion

        #region Private Methods



        #endregion
    }
}
