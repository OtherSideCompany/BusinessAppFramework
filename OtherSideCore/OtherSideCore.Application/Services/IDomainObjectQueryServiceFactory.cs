using OtherSideCore.Domain.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Application.Services
{
    public interface IDomainObjectQueryServiceFactory
    {
        IDomainObjectQueryService<T> CreateDomainObjectQueryService<T>() where T : DomainObject, new();
    }
}
