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
   public class ModelObjectViewModelBase : ViewModelBase
   {
      #region Fields

      private ModelObjectBase m_ModelObjectBase;

      private ModelObjectViewModelBase m_ParentModelObjectViewModelBase;
      private ObservableCollection<ModelObjectViewModelBase> m_ChildrenModelObjectViewModelBase;

      private Command m_SaveChangesCommand;
      private Command m_CancelChangesCommand;
      private Command m_DeleteCommand;
      private Command m_DisplayInExternalWindowCommand;

      #endregion

      #region Properties

      public ModelObjectBase ModelObjectBase
      {
         get
         {
            return m_ModelObjectBase;
         }
         set
         {
            if (value != m_ModelObjectBase)
            {
               m_ModelObjectBase = value;
               OnPropertyChanged(nameof(ModelObjectBase));
            }
         }
      }

      public ModelObjectViewModelBase ParentModelObjectViewModelBase
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

      public ObservableCollection<ModelObjectViewModelBase> ChildrenModelObjectViewModelBase
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

      public ModelObjectViewModelBase(ModelObjectBase modelObjectBase) : base()
      {
         ModelObjectBase = modelObjectBase;
         ChildrenModelObjectViewModelBase = new ObservableCollection<ModelObjectViewModelBase>();
      }

      public ModelObjectViewModelBase(ModelObjectBase modelObjectBase, ModelObjectViewModelBase parentModelObjectViewModelBase) : this(modelObjectBase)
      {
         ParentModelObjectViewModelBase = parentModelObjectViewModelBase;
      }

      #endregion

      #region Methods

      public virtual bool CanSaveChanges()
      {
         return ModelObjectBase.CanSaveChanges();
      }

      public virtual void SaveChanges()
      {
         ModelObjectBase.Save();
         ModelObjectBase.Load();
      }

      public virtual bool CanCancelChanges()
      {
         return ModelObjectBase.CanCancelChanges();
      }

      public virtual void CancelChanges()
      {
         ModelObjectBase.Load();
      }

      private bool CanExecuteDeleteCommand(object parameter)
      {
         return (parameter as ModelObjectViewModelBase) != null && (parameter as ModelObjectViewModelBase).ModelObjectBase.CanBeDeleted();
      }

      private void ExecuteDeleteCommand(object parameter)
      {
         (parameter as ModelObjectBase).Delete();
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
