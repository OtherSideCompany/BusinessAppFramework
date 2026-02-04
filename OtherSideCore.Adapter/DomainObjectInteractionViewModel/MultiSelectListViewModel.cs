using CommunityToolkit.Mvvm.ComponentModel;
using OtherSideCore.Domain.DomainObjects;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace OtherSideCore.Adapter.DomainObjectInteractionViewModel
{
   public abstract class MultiSelectListViewModel<T> : ObservableObject, IMultiSelectListViewModel where T : DomainObject, new()
   {
      #region Fields

      protected MultiSelectListViewModelDependencies _multiSelectListViewModelDependencies;

      private bool _hasUnsavedChanges;
      private bool _isInitializingItemsSelectionFromContext;
      protected DomainObjectViewModel _contextViewModel;

      #endregion

      #region Properties

      public bool HasUnsavedChanges
      {
         get => _hasUnsavedChanges;
         private set => SetProperty(ref _hasUnsavedChanges, value);
      }

      public ObservableCollection<DomainObjectViewModel> Items { get; } = new();

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public MultiSelectListViewModel(MultiSelectListViewModelDependencies multiSelectListViewModelDependencies)
      {
         _multiSelectListViewModelDependencies = multiSelectListViewModelDependencies;
      }

      #endregion

      #region Public Methods

      public async Task InitializeAsync(DomainObjectViewModel contextViewModel)
      {
         _contextViewModel = contextViewModel;

         await LoadOptionsAsync();
         await InitializeItemsSelectionFromContextAsync();
      }           

      public bool CanCancelChanges()
      {
         return HasUnsavedChanges;
      }

      public async Task CancelChangesAsync()
      {
         await InitializeItemsSelectionFromContextAsync();
         HasUnsavedChanges = false;
      }

      public bool CanSaveChanges()
      {
         return HasUnsavedChanges;
      }

      public async Task SaveChangesAsync()
      {
         var ids = Items.Where(item => item.IsSelected).Select(item => item.DomainObject.Id).ToList();
         await SetContextItemIdsAsync(ids);
         HasUnsavedChanges = false;
      }

      public void Dispose()
      {
         Items.ToList().ForEach(item => item.PropertyChanged += DomainObjectViewModel_PropertyChanged);
         Items.Clear();
      }

      #endregion

      #region Private Methods

      private void DomainObjectViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
      {
         if (!_isInitializingItemsSelectionFromContext && e.PropertyName.Equals(nameof(DomainObjectViewModel.IsSelected)))
         {
            HasUnsavedChanges = true;
         }
      }

      private async Task LoadOptionsAsync()
      {
         Items.Clear();

         var domainObjectService = _multiSelectListViewModelDependencies.DomainObjectServiceFactory.CreateDomainObjectService<T>();
         var (_, domainObjects) = await DomainObjectServiceHelper.TryGetAllAsync(domainObjectService, _multiSelectListViewModelDependencies.UserDialogService, _multiSelectListViewModelDependencies.LocalizationService);
         domainObjects = domainObjects.OrderBy(domainObject => domainObject.ToString()).ToList();

         domainObjects.ForEach(t => Items.Add(_multiSelectListViewModelDependencies.DomainObjectViewModelFactory.CreateViewModel(t)));
         Items.ToList().ForEach(item => item.PropertyChanged += DomainObjectViewModel_PropertyChanged);
      }

      private async Task InitializeItemsSelectionFromContextAsync()
      {
         _isInitializingItemsSelectionFromContext = true;

         var contextItemIds = await GetContextItemIdsAsync();

         foreach (var item in Items)
         {
            item.IsSelected = contextItemIds.Contains(item.DomainObject.Id);
         }

         _isInitializingItemsSelectionFromContext = false;
      }

      protected abstract Task<IEnumerable<int>> GetContextItemIdsAsync();
      protected abstract Task SetContextItemIdsAsync(List<int> ids);

      #endregion
   }
}
