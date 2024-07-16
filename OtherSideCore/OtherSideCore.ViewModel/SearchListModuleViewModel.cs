using OtherSideCore.Model.DatabaseFields;
using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Threading;
using OtherSideCore.Model.Services;
using OtherSideCore.Model.ModelObjects;
using OtherSideCore.Model.Repositories;
using OtherSideCore.Model;

namespace OtherSideCore.ViewModel
{
   public abstract class SearchListModuleViewModel<T> : ObservableObject, IDisposable where T : ModelObject
   {
      #region Fields

      //protected User _authenticatedUser;

      private List<DatabaseField> _databaseFields;

      private RepositoryManager<T> _repositoryManager;

      //private Repository<ModelObject, Data.Entities.EntityBase> _repository;

      #endregion

      #region Properties

      public bool IsAnyDatabaseFieldDirty
      {
         get
         {
            return _databaseFields.Any(dbf => dbf.IsDirty);
         }
      }

      public RepositoryManager<T> RepositoryManager
      {
         get => _repositoryManager;
         set => SetProperty(ref _repositoryManager, value);
      }

      //public User AuthenticatedUser
      //{
      //   get => _authenticatedUser;
      //   set => SetProperty(ref _authenticatedUser, value);
      //}

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public SearchListModuleViewModel()
      {
         //AuthenticatedUser = authenticatedUser;
         _databaseFields = new List<DatabaseField>();
         //_repository = repository;
      }

      #endregion

      #region Methods

      protected virtual async Task SelectedSearchResultChangedAsync(CancellationToken cancellation)
      {
         RegisterDatabaseFields();
      }

      private void UnregisterDatabaseFields()
      {
         foreach (var databaseField in _databaseFields)
         {
            databaseField.PropertyChanged -= DatabaseField_OnPropertyChanged;
         }

         _databaseFields.Clear();
      }

      private void RegisterDatabaseFields()
      {
         //UnregisterDatabaseFields();

         //foreach (var databaseField in RepositorySearch.SelectedModelObject.GetDatabaseFields())
         //{
         //   databaseField.PropertyChanged += DatabaseField_OnPropertyChanged;
         //   _databaseFields.Add(databaseField);
         //}
      }

      private void DatabaseField_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
      {
         //if (e.PropertyName.Equals(nameof(DatabaseField.IsDirty)))
         //{
         //   OnPropertyChanged(nameof(IsAnyDatabaseFieldDirty));

         //   RepositorySearchViewModel.SelectedSearchResultViewModel.NotifyCommandsCanExecuteChanged();

         //   if (IsAnyDatabaseFieldDirty)
         //   {
         //      RepositorySearch.LockSelection();
         //   }
         //   else
         //   {
         //      RepositorySearch.UnlockSelection();
         //   }
         //}
      }

      public virtual void Dispose()
      {
         //RepositorySearch.Dispose();

         //UnregisterDatabaseFields();
      }

      #endregion
   }
}
