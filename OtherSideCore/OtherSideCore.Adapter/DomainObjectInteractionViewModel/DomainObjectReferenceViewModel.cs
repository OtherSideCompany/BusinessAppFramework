using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Adapter.Services;
using OtherSideCore.Application;

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
         private set => SetProperty(ref _domainObjectReference, value);
      }

      #endregion

      

      #region Commands

      public AsyncRelayCommand<DomainObjectReferenceViewModel> DeleteDomainObjectReferenceAsyncCommand { get; private set; }

      #endregion

      #region Constructor

      public DomainObjectReferenceViewModel(
         DomainObjectReference domainObjectReference,
         IDomainObjectInteractionService domainObjectInteractionService)
      {
         DomainObjectReference = domainObjectReference;
         _domainObjectInteractionService = domainObjectInteractionService;


      }

      #endregion

      #region Public Methods

      public async Task DisplayReferenceAsync()
      {
         //await _domainObjectInteractionService.DisplayDomainObjectAsync(DomainObjectReference.DomainObjectId, DomainObjectReference.ReferenceType, DisplayType.SubWindow);
      }

      public async Task DisplayReferenceTreeViewAsync()
      {
         //await _domainObjectInteractionService.DisplayDomainObjectTreeViewAsync(DomainObjectReference.DomainObjectId, DomainObjectReference.ReferenceType, DisplayType.SubWindow);
      }

      public async Task DisplayReferenceDetailsAsync()
      {
         //await _domainObjectInteractionService.DisplayDomainObjectDetailsEditorViewAsync(DomainObjectReference.DomainObjectId, DomainObjectReference.ReferenceType, DisplayType.Modal);
      }

      public void Dispose()
      {
         
      }

      #endregion

      #region Private Methods



      #endregion

   }
}
