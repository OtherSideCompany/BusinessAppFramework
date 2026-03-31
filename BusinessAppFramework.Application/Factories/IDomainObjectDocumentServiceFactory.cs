using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Domain.DomainObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAppFramework.Application.Factories
{
   public interface IDomainObjectDocumentServiceFactory
   {
      IDomainObjectDocumentService<T> CreateDomainObjectDocumentService<T>() where T : DomainObject, new();
   }
}
