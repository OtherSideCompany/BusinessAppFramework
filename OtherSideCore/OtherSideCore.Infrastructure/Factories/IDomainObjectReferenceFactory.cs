using OtherSideCore.Application;
using OtherSideCore.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Infrastructure.Factories
{
   public interface IDomainObjectReferenceFactory
   {
      DomainObjectReference CreateDomainObjectReference(EntityBase entityBase);
   }
}
