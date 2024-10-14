using OtherSideCore.Domain.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Application.Services
{
   public interface IDomainObjectServiceFactory
   {
      IDomainObjectService<T> CreateDomainObjectService<T>() where T : DomainObject, new();
   }
}
