using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OtherSideCore.ViewModel
{
   public abstract class ModelObjectViewModel : ViewModelBase
   {
      #region Fields

      private ModelObject m_ModelObject;

      private ModelObjectViewModel m_ParentModelObjectViewModelBase;
      private ObservableCollection<ModelObjectViewModel> m_ChildrenModelObjectViewModelBase;

      #endregion

      #region Properties

      public ModelObject ModelObject
      {
         get => m_ModelObject;
         set => SetProperty(ref m_ModelObject, value);
      }

      public ModelObjectViewModel ParentModelObjectViewModelBase
      {
         get => m_ParentModelObjectViewModelBase;
         set => SetProperty(ref m_ParentModelObjectViewModelBase, value);
      }

      public ObservableCollection<ModelObjectViewModel> ChildrenModelObjectViewModelBase
      {
         get => m_ChildrenModelObjectViewModelBase;
         set => SetProperty(ref m_ChildrenModelObjectViewModelBase, value);
      }

      #endregion

      #region Commands

      public AsyncRelayCommand SaveChangesAsyncCommand { get; private set; }
      public AsyncRelayCommand CancelChangesAsyncCommand { get; private set; }
      public AsyncRelayCommand DeleteAsyncCommand { get; private set; }
      public RelayCommand DisplayInExternalWindowCommand { get; private set; }

      #endregion

      #region Constructor

      public ModelObjectViewModel(ModelObject modelObject) : base()
      {
         SaveChangesAsyncCommand = new AsyncRelayCommand(SaveChangesAsync, CanSaveChanges);
         CancelChangesAsyncCommand = new AsyncRelayCommand(CancelChangesAsync, CanCancelChanges);
         DeleteAsyncCommand = new AsyncRelayCommand(DeleteAsync, CanExecuteDelete);
         DisplayInExternalWindowCommand = new RelayCommand(DisplayInExternalWindow);

         ModelObject = modelObject;
         ChildrenModelObjectViewModelBase = new ObservableCollection<ModelObjectViewModel>();
      }

      public ModelObjectViewModel(ModelObject modelObject, ModelObjectViewModel parentModelObjectViewModel) : this(modelObject)
      {
         ParentModelObjectViewModelBase = parentModelObjectViewModel;
      }

      #endregion

      #region Methods

      public void NotifyCommandsCanExecuteChanged()
      {
         SaveChangesAsyncCommand.NotifyCanExecuteChanged();
         CancelChangesAsyncCommand.NotifyCanExecuteChanged();
         DeleteAsyncCommand.NotifyCanExecuteChanged();
      }

      public virtual bool CanSaveChanges()
      {
         return ModelObject.CanSaveChanges();
      }

      public virtual async Task SaveChangesAsync()
      {
         await ModelObject.SaveAsync();
         await ModelObject.LoadAsync();
      }

      public virtual bool CanCancelChanges()
      {
         return ModelObject.CanCancelChanges();
      }

      public virtual async Task CancelChangesAsync()
      {
         await ModelObject.LoadAsync();
      }

      private bool CanExecuteDelete()
      {
         return ModelObject.CanBeDeleted();
      }

      private async Task DeleteAsync()
      {
         await ModelObject.DeleteAsync();
      }

      protected abstract void DisplayInExternalWindow();

      protected override void ExecuteToggleExpandCommand()
      {
         base.ExecuteToggleExpandCommand();

         if (IsExpanded)
         {
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
               ExpandChildren();
            }
         }
      }

      public virtual void ExpandChildren()
      {
         foreach (var viewModel in ChildrenModelObjectViewModelBase)
         {
            viewModel.IsExpanded = true;
            viewModel.ExpandChildren();
         }
      }

      public override void Dispose()
      {
         foreach (var viewModel in ChildrenModelObjectViewModelBase)
         {
            viewModel.Dispose();
         }

         ChildrenModelObjectViewModelBase.Clear();
      }

      #endregion
   }
}
