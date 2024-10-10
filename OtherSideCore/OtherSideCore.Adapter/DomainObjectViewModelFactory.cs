using AutoMapper;
using OtherSideCore.Domain.DomainObjects;
using OtherSideCore.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Adapter
{
   public abstract class DomainObjectViewModelFactory : IDomainObjectViewModelFactory
   {
      #region Fields

      protected IGlobalDataService _globalDataService;
      protected IMapper _mapper;

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectViewModelFactory(IGlobalDataService globalDataService, IMapper mapper)
      {
         _globalDataService = globalDataService;
         _mapper = mapper;
      }

      #endregion

      #region Public Methods

      public abstract DomainObjectViewModel CreateViewModel(DomainObject domainObject);

      #endregion

      #region Private Methods



      #endregion
   }
}
