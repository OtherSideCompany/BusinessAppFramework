using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Adapter.Relations
{
   public interface IDomainObjectEntityTypeMap
   {
      void Register(Type domainType, Type entityType);
      Type GetEntityType(Type domainType);
      Type GetDomainType(Type entityType);
   }
}
