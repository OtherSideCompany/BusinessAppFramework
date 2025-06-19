using OtherSideCore.Adapter.DomainObjectInteraction;
using OtherSideCore.Application.Search;
using OtherSideCore.Domain.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Adapter.Factories
{
   public interface IDomainObjectViewModelFactory
   {
      void RegisterViewModel<T>(Func<DomainObject, DomainObjectViewModel> factory) where T : DomainObject, new();
      DomainObjectViewModel CreateViewModel(DomainObject domainObject);
   }
}
