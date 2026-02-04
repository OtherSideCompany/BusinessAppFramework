using CommunityToolkit.Mvvm.Input;
using OtherSideCore.Domain.ModelObjects;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
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

      public string CreationDescription => GetHistoryDescription(ModelObject.CreationDate.Value, ModelObject.CreatedBy);

      public string ModificationDescription => GetHistoryDescription(ModelObject.LastModifiedDateTime.Value, ModelObject.LastModifiedBy);

      #endregion

      #region Commands

      public RelayCommand DisplayInExternalWindowCommand { get; private set; }

      #endregion

      #region Constructor

      public ModelObjectViewModel(ModelObject modelObject) : base()
      {
         DisplayInExternalWindowCommand = new RelayCommand(DisplayInExternalWindow);

         ModelObject = modelObject;
         ChildrenModelObjectViewModelBase = new ObservableCollection<ModelObjectViewModel>();

         ModelObject.PropertyChanged += ModelObject_PropertyChanged;
      }

      public ModelObjectViewModel(ModelObject modelObject, ModelObjectViewModel parentModelObjectViewModel) : this(modelObject)
      {
         ParentModelObjectViewModelBase = parentModelObjectViewModel;
      }

      #endregion

      #region Public Methods

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
         ModelObject.PropertyChanged -= ModelObject_PropertyChanged;

         foreach (var viewModel in ChildrenModelObjectViewModelBase)
         {
            viewModel.Dispose();
         }

         ChildrenModelObjectViewModelBase.Clear();
      }

      #endregion

      #region private Methods

      protected virtual void DisplayInExternalWindow() { }

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

      private void ModelObject_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
      {
         OnPropertyChanged(nameof(CreationDescription));
         OnPropertyChanged(nameof(ModificationDescription));
      }

      private string GetHistoryDescription(DateTime dateTime, User user)
      {
         var creationDescription = dateTime.ToString("dd/MM/yyyy, HH:mm, ");

         if (user != null)
         {
            creationDescription += user.FirstName.Value + " " + user.LastName.Value;
         }
         else
         {
            creationDescription += "Inconnu";
         }

         return creationDescription;
      }

      #endregion
   }
}
