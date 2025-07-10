using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Adapter.DomainObjectInteractionViewModel;
using OtherSideCore.Adapter.Services;
using OtherSideCore.Application.Factories;
using OtherSideCore.Appplication.Services;
using OtherSideCore.Domain.DomainObjects;
using System.Collections.ObjectModel;

namespace OtherSideCore.Adapter.Relations
{
   public class RelationEntryViewModel<T> : ObservableObject, IDisposable where T : DomainObject, new()
   {
      #region Fields

      private IDomainObjectServiceFactory _domainObjectServiceFactory;
      private IDomainObjectInteractionService _domainObjectInteractionService;
      private IUserDialogService _userDialogService;

      private DomainObjectViewModel _domainObjectViewModel;
      private IRelationEntry _relationEntry;
      private ObservableCollection<DomainObjectReferenceViewModel> _domainObjectReferenceViewModels;
      private string _relationName;

      #endregion

      #region Properties

      public string RelationName
      {
         get => _relationName;
         set => SetProperty(ref _relationName, value);
      }

      public IRelationEntry RelationEntry
      {
         get => _relationEntry;
         set => SetProperty(ref _relationEntry, value);
      }

      public ObservableCollection<DomainObjectReferenceViewModel> DomainObjectReferenceViewModels
      {
         get => _domainObjectReferenceViewModels;
         set => SetProperty(ref _domainObjectReferenceViewModels, value);
      }

      #endregion

      #region Events

      public event EventHandler<DomainObjectReferenceViewModel> DomainObjectReferenceDeletedEvent;

      #endregion

      #region Commands

      public AsyncRelayCommand<DomainObjectReferenceViewModel> DeleteDomainObjectReferenceAsyncCommand { get; private set; }

      #endregion

      #region Constructor

      public RelationEntryViewModel(
         DomainObjectViewModel domainObjectViewModel,
         IRelationEntry relationEntry,
         ILocalizationService localizationService,
         IDomainObjectServiceFactory domainObjectServiceFactory,
         IDomainObjectInteractionService domainObjectInteractionService,
         IUserDialogService userDialogService)
      {
         _domainObjectServiceFactory = domainObjectServiceFactory;
         _domainObjectInteractionService = domainObjectInteractionService;
         _userDialogService = userDialogService;

         _domainObjectViewModel = domainObjectViewModel;
         RelationEntry = relationEntry;
         DomainObjectReferenceViewModels = new ObservableCollection<DomainObjectReferenceViewModel>();
         RelationName = localizationService.GetString(relationEntry.RelationKey.Key);

         DeleteDomainObjectReferenceAsyncCommand = new AsyncRelayCommand<DomainObjectReferenceViewModel>(DeleteDomainObjectReferenceAsync);
      }

      #endregion

      #region Public Methods

      public async Task InitializeAsync()
      {
         UnloadDomainObjectReferenceViewModels();

         var domainObjectService = _domainObjectServiceFactory.CreateDomainObjectService<T>();
         var domainObjectReferences = await domainObjectService.GetDomainObjectReferencesAsync(RelationEntry.RelationKey, _domainObjectViewModel.DomainObject.Id);

         foreach (var reference in domainObjectReferences)
         {
            var viewModel = new DomainObjectReferenceViewModel(reference, _domainObjectInteractionService);
            DomainObjectReferenceViewModels.Add(viewModel);
         }
      }

      public void Dispose()
      {
         UnloadDomainObjectReferenceViewModels();
      }

      #endregion

      #region Private Methods

      private void UnloadDomainObjectReferenceViewModels()
      {
         DomainObjectReferenceViewModels.ToList().ForEach(vm => vm.Dispose());
         DomainObjectReferenceViewModels.Clear();
      }

      protected virtual async Task DeleteDomainObjectReferenceAsync(DomainObjectReferenceViewModel? domainObjectReferenceViewModel)
      {
         if (domainObjectReferenceViewModel != null && _userDialogService.Confirm("Supprimer la référence ?"))
         {
            var domainObjectService = _domainObjectServiceFactory.CreateDomainObjectService<T>();
            await domainObjectService.DeleteDomainObjectReferenceAsync(RelationEntry.RelationKey, _domainObjectViewModel.DomainObject.Id, domainObjectReferenceViewModel.DomainObjectReference);

            DomainObjectReferenceViewModels.Remove(domainObjectReferenceViewModel);

            DomainObjectReferenceDeletedEvent?.Invoke(this, domainObjectReferenceViewModel);
         }         
      }

      #endregion
   }
}
