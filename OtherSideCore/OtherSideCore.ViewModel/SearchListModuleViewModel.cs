using OtherSideCore.Model.DatabaseFields;
using OtherSideCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace OtherSideCore.ViewModel
{
   public abstract class SearchListModuleViewModel : ObservableObject, IDisposable
   {
      #region Fields

      private List<DatabaseField> m_DatabaseFields;

      private ModelObjectListSearchViewModel m_ModelObjectListSearchViewModel;

      #endregion

      #region Properties

      public bool IsAnyDatabaseFieldDirty
      {
         get
         {
            return m_DatabaseFields.Any(dbf => dbf.IsDirty);
         }
      }

      public ModelObjectListSearchViewModel ModelObjectListSearchViewModel
      {
         get => m_ModelObjectListSearchViewModel;
         set => SetProperty(ref m_ModelObjectListSearchViewModel, value);
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public SearchListModuleViewModel()
      {
         m_DatabaseFields = new List<DatabaseField>();
      }

      #endregion

      #region Methods

      protected async Task SelectedSearchResultChangedAsync()
      {
         RegisterDatabaseFields();
      }

      private void UnregisterDatabaseFields()
      {
         foreach (var databaseField in m_DatabaseFields)
         {
            databaseField.PropertyChanged -= DatabaseField_OnPropertyChanged;
         }

         m_DatabaseFields.Clear();
      }

      private void RegisterDatabaseFields()
      {
         UnregisterDatabaseFields();

         foreach (var databaseField in ModelObjectListSearchViewModel.SelectedSearchResultViewModel.ModelObject.GetDatabaseFields())
         {
            databaseField.PropertyChanged += DatabaseField_OnPropertyChanged;
            m_DatabaseFields.Add(databaseField);
         }
      }

      private void DatabaseField_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
      {
         if (e.PropertyName.Equals(nameof(DatabaseField.IsDirty)))
         {
            OnPropertyChanged(nameof(IsAnyDatabaseFieldDirty));

            ModelObjectListSearchViewModel.SelectedSearchResultViewModel.NotifyCommandsCanExecuteChanged();

            if (IsAnyDatabaseFieldDirty)
            {
               ModelObjectListSearchViewModel.ModelObjectListSearch.LockSelection();
            }
            else
            {
               ModelObjectListSearchViewModel.ModelObjectListSearch.UnlockSelection();
            }
         }
      }

      public virtual void Dispose()
      {
         ModelObjectListSearchViewModel.Dispose();

         UnregisterDatabaseFields();
      }

      #endregion
   }
}
