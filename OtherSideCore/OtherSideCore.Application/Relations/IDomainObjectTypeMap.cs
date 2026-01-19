using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Application.Relations
{
    public interface IDomainObjectTypeMap
    {
        void Register(Type domainType, Type entityType, Type searchResultType);
        Type GetEntityTypeFromDomainObjectType(Type domainType);
        Type GetDomainTypeFromEntityType(Type entityType);
        Type GetSearchResultTypeFromDomainType(Type domainType);
    }
}
