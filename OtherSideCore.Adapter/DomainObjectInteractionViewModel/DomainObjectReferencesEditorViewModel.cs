using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Adapter.Relations;
using OtherSideCore.Adapter.Services;
using OtherSideCore.Domain.DomainObjects;
using System.Collections.ObjectModel;

namespace OtherSideCore.Adapter.DomainObjectInteractionViewModel
{
   public class DomainObjectReferencesEditorViewModel<T> : ObservableObject, IDomainObjectReferencesEditorViewModel, IDomainObjectInteractionHost, IDisposable where T : DomainObject, new()
   {
      #region Fields

      private DomainObjectEditorViewModelDependencies _domainObjectEditorViewModelDependencies;
      private Type _sourceEntityType;
      private ObservableCollection<RelationEntryViewModel<T>> _relationEntryViewModels;
      private DomainObjectViewModel _domainObjectViewModel;

      #endregion

      #region Properties

      public ObservableCollection<RelationEntryViewModel<T>> RelationEntryViewModels
      {
         get => _relationEntryViewModels;
         set => SetProperty(ref _relationEntryViewModels, value);
      }

      #endregion

      #region Events

      public event EventHandler DomainObjectReferencesModified;

      #endregion

      #region Commands      

      public IDomainObjectInteractionService DomainObjectInteractionService => _domainObjectEditorViewModelDependencies.DomainObjectInteractionService;

      #endregion

      #region Constructor

      public DomainObjectReferencesEditorViewModel(
         DomainObjectViewModel domainObjectViewModel,
         DomainObjectEditorViewModelDependencies domainObjectEditorViewModelDependencies)
      {
         _domainObjectViewModel = domainObjectViewModel;
         _domainObjectEditorViewModelDependencies = domainObjectEditorViewModelDependencies;

         _sourceEntityType = _domainObjectEditorViewModelDependencies.DomainObjectEntityTypeMap.GetEntityType(_domainObjectViewModel.DomainObject.GetType());

         RelationEntryViewModels = new ObservableCollection<RelationEntryViewModel<T>>();

         var relations = _domainObjectEditorViewModelDependencies.RelationResolver.GetEntriesBySourceType(_sourceEntityType);

         foreach (var relation in relations)
         {
            var viewModel = new RelationEntryViewModel<T>(
               _domainObjectViewModel,
               relation,
               _domainObjectEditorViewModelDependencies.LocalizationService,
               _domainObjectEditorViewModelDependencies.DomainObjectServiceFactory,
               _domainObjectEditorViewModelDependencies.DomainObjectInteractionService,
               _domainObjectEditorViewModelDependencies.UserDialogService);

            viewModel.DomainObjectReferenceDeletedEvent += ViewModel_DomainObjectReferenceDeletedEvent;

            RelationEntryViewModels.Add(viewModel);
         }
      }      

      #endregion

      #region Public Methods

      public async Task InitializeAsync()
      {
         foreach (var relationEntryViewModel in RelationEntryViewModels)
         {
            await relationEntryViewModel.InitializeAsync();
         }
      }

      public void Dispose()
      {
         foreach (var viewModel in RelationEntryViewModels)
         {
            viewModel.DomainObjectReferenceDeletedEvent -= ViewModel_DomainObjectReferenceDeletedEvent;
            viewModel.Dispose();
         }
         
         RelationEntryViewModels.Clear();
      }

      #endregion

      #region Private Methods

      private void ViewModel_DomainObjectReferenceDeletedEvent(object? sender, DomainObjectReferenceViewModel e)
      {
         DomainObjectReferencesModified?.Invoke(this, EventArgs.Empty);
      }

      #endregion
   }
}