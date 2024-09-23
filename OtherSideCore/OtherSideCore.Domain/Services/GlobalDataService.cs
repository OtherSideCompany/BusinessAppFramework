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

      private bool _areDataLoaded;

      #endregion

      #region Properties

      public bool AreDataLoaded
      {
         get => _areDataLoaded;
         set => SetProperty(ref _areDataLoaded, value);
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public GlobalDataService()
      {

      }

      #endregion

      #region Public Methods

      public abstract Task LoadGlobalDataAsync(IRepositoryFactory repositoryFactory);

      public abstract void UnloadData();

      #endregion

      #region Private Methods



      #endregion
   }
}
