using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Application;
using OtherSideCore.Appplication.Services;

namespace OtherSideCore.Adapter.DomainObjectInteractionViewModel
{
   public class DomainObjectReferenceViewModel : ObservableObject, IDisposable
   {
      #region Fields

      private DomainObjectReference _domainObjectReference;
      private IDomainObjectInteractionService _domainObjectInteractionService;

      #endregion

      #region Properties

      public DomainObjectReference DomainObjectReference
      {
         get => _domainObjectReference;
         private set { SetProperty(ref _domainObjectReference, value); OnPropertyChanged(nameof(IsNameDefault)); }
      }

      public bool IsNameDefault => String.IsNullOrEmpty(DomainObjectReference.Name) || DomainObjectReference.Name.Equals("-NA-");

      #endregion

      #region Commands

      public AsyncRelayCommand DisplayReferenceAsyncCommand { get; private set; }

      #endregion

      #region Constructor

      public DomainObjectReferenceViewModel(
         DomainObjectReference domainObjectReference,
         IDomainObjectInteractionService domainObjectInteractionService)
      {
         DomainObjectReference = domainObjectReference;
         _domainObjectInteractionService = domainObjectInteractionService;

         DisplayReferenceAsyncCommand = new AsyncRelayCommand(DisplayReferenceAsync);
      }

      #endregion

      #region Public Methods

      private async Task DisplayReferenceAsync()
      {
         await _domainObjectInteractionService.DisplayDomainObjectAsync(DomainObjectReference.DomainObjectId, DomainObjectReference.ReferenceType, DisplayType.SubWindow);
      }
       
      public void Dispose()
      {
         
      }

      #endregion

      #region Private Methods



      #endregion

   }
}
