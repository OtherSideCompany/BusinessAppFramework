using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Adapter.DomainObjectInteractionViewModel
{
   public class ReferenceSelectedEventArgs : EventArgs
   {
      public int DomainObjectId { get; }
      public Type ReferenceType { get; }

      public ReferenceSelectedEventArgs(int domainObjectId, Type referenceType)
      {
         DomainObjectId = domainObjectId;
         ReferenceType = referenceType;
      }
   }
}
