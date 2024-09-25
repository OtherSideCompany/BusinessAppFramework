using CommunityToolkit.Mvvm.ComponentModel;
using OtherSideCore.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Domain.Services
{
   public abstract class GlobalDataService : ObservableObject, IGlobalDataService
   {
      #region Fields

      protected IRepositoryFactory _repositoryFactory;

      #endregion

      #region Properties


      #endregion

      #region Commands



      #endregion

      #region Constructor

      public GlobalDataService()
      {

      }

      #endregion

      #region Public Methods

      public void SetRepositoryFactory(IRepositoryFactory repositoryFactory)
      {
         _repositoryFactory = repositoryFactory;
      }

      public abstract Task LoadGlobalDataAsync();

      public abstract void UnloadData();

      #endregion

      #region Private Methods



      #endregion
   }
}
