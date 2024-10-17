using OtherSideCore.Application.DomainObjectBrowser;
using OtherSideCore.Application.Services;
using OtherSideCore.Domain.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Adapter.DomainObjectBrowser
{
   public interface IDomainObjectsSearchViewModelFactory
   {
      IDomainObjectSearchViewModel CreateDomainObjectSearchViewModel<T>(DomainObjectSearch<T> domainObjectSearch, IDomainObjectViewModelFactory domainObjectViewModelFactory) where T : DomainObject, new();
   }
}
