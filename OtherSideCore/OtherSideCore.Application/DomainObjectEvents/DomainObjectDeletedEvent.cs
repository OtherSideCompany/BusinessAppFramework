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

      public int DeletedDomainObjectId { get; set; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectDeletedEvent(DomainObject domainObject, int deletedDomainObjectId) : base(domainObject)
      {
         DeletedDomainObjectId = deletedDomainObjectId;
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
