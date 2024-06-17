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
   public class ModelObjectViewModel : ViewModelBase
   {
      #region Fields

      private ModelObject m_ModelObject;

      private ModelObjectViewModel m_ParentModelObjectViewModelBase;
      private ObservableCollection<ModelObjectViewModel> m_ChildrenModelObjectViewModelBase;

      private Command m_SaveChangesCommand;
      private Command m_CancelChangesCommand;
      private Command m_DeleteCommand;
      private Command m_DisplayInExternalWindowCommand;

      #endregion

      #region Properties

      public ModelObject ModelObject
      {
         get
         {
            return m_ModelObject;
         }
         set
         {
            if (value != m_ModelObject)
            {
               m_ModelObject = value;
               OnPropertyChanged(nameof(ModelObject));
            }
         }
      }

      public ModelObjectViewModel ParentModelObjectViewModelBase
      {
         get
         {
            return m_ParentModelObjectViewModelBase;
         }
         set
         {
            if (value != m_ParentModelObjectViewModelBase)
            {
               m_ParentModelObjectViewModelBase = value;
               OnPropertyChanged(nameof(ParentModelObjectViewModelBase));
            }
         }
      }

      public ObservableCollection<ModelObjectViewModel> ChildrenModelObjectViewModelBase
      {
         get
         {
            return m_ChildrenModelObjectViewModelBase;
         }
         set
         {
            if (value != m_ChildrenModelObjectViewModelBase)
            {
               m_ChildrenModelObjectViewModelBase = value;
               OnPropertyChanged(nameof(ChildrenModelObjectViewModelBase));
            }
         }
      }

      #endregion

      #region Commands

      public Command SaveChangesCommand
      {
         get
         {
            if (m_SaveChangesCommand == null)
            {
               m_SaveChangesCommand = new Command(ExecuteSaveChangesCommand, CanExecuteSaveChangesCommand);
            }
            return m_SaveChangesCommand;
         }
      }

      public Command CancelChangesCommand
      {
         get
         {
            if (m_CancelChangesCommand == null)
            {
               m_CancelChangesCommand = new Command(ExecuteCancelChangesCommand, CanExecuteCancelChangesCommand);
            }
            return m_CancelChangesCommand;
         }
      }

      public Command DeleteCommand
      {
         get
         {
            if (m_DeleteCommand == null)
            {
               m_DeleteCommand = new Command(ExecuteDeleteCommand, CanExecuteDeleteCommand);
            }
            return m_DeleteCommand;
         }
      }

      public Command DisplayInExternalWindowCommand
      {
         get
         {
            if (m_DisplayInExternalWindowCommand == null)
            {
               m_DisplayInExternalWindowCommand = new Command(ExecuteDisplayInExternalWindowCommand, CanExecuteDisplayInExternalWindowCommand);
            }

            return m_DisplayInExternalWindowCommand;
         }
      }

      #endregion

      #region Constructor

      public ModelObjectViewModel(ModelObject modelObject) : base()
      {
         ModelObject = modelObject;
         ChildrenModelObjectViewModelBase = new ObservableCollection<ModelObjectViewModel>();
      }

      public ModelObjectViewModel(ModelObject modelObject, ModelObjectViewModel parentModelObjectViewModel) : this(modelObject)
      {
         ParentModelObjectViewModelBase = parentModelObjectViewModel;
      }

      #endregion

      #region Methods

      public virtual bool CanSaveChanges()
      {
         return ModelObject.CanSaveChanges();
      }

      public virtual void SaveChanges()
      {
         ModelObject.Save();
         ModelObject.Load();
      }

      public virtual bool CanCancelChanges()
      {
         return ModelObject.CanCancelChanges();
      }

      public virtual void CancelChanges()
      {
         ModelObject.Load();
      }

      private bool CanExecuteDeleteCommand(object parameter)
      {
         return (parameter as ModelObjectViewModel) != null && (parameter as ModelObjectViewModel).ModelObject.CanBeDeleted();
      }

      private void ExecuteDeleteCommand(object parameter)
      {
         (parameter as ModelObjectViewModel).ModelObject.Delete();
      }

      private bool CanExecuteSaveChangesCommand(object parameter)
      {
         return CanSaveChanges();
      }

      private void ExecuteSaveChangesCommand(object parameter)
      {
         SaveChanges();
      }

      private bool CanExecuteCancelChangesCommand(object parameter)
      {
         return CanCancelChanges();
      }

      private void ExecuteCancelChangesCommand(object parameter)
      {
         CancelChanges();
      }

      protected virtual bool CanExecuteDisplayInExternalWindowCommand(object parameter)
      {
         return true;
      }

      protected virtual void ExecuteDisplayInExternalWindowCommand(object parameter) { }

      protected override void ExecuteToggleExpandCommand(object parameter)
      {
         base.ExecuteToggleExpandCommand(parameter);

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
