using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace OtherSideCore.ViewModel
{
   public class ModelViewModel : ObservableObject, IDisposable
   {
      #region Fields

      private Model.Model m_Model;

      private ModuleBaseViewModel m_LoadedModuleBaseViewModel;

      private ObservableCollection<ModuleBaseViewModel> m_ModuleBaseViewModels;
      private ObservableCollection<ModuleBaseViewModel> m_QuickNavigationModuleViewModels;

      #endregion

      #region Properties

      public Model.Model Model
      {
         get => m_Model;
         set => SetProperty(ref m_Model, value);
      }

      public ModuleBaseViewModel LoadedModuleBaseViewModel
      {
         get => m_LoadedModuleBaseViewModel;
         set => SetProperty(ref m_LoadedModuleBaseViewModel, value);
      }

      public ObservableCollection<ModuleBaseViewModel> ModuleBaseViewModels
      {
         get => m_ModuleBaseViewModels;
         set => SetProperty(ref m_ModuleBaseViewModels, value);
      }

      public ObservableCollection<ModuleBaseViewModel> QuickNavigationModuleViewModels
      {
         get => m_QuickNavigationModuleViewModels;
         set => SetProperty(ref m_QuickNavigationModuleViewModels, value);
      }

      #endregion

      #region Commands

      public RelayCommand<ModuleBaseViewModel> LoadModuleCommand { get; private set; }
      public RelayCommand<object> LoadFilteredModuleCommand { get; private set; }

      #endregion

      #region Constructor

      public ModelViewModel(Model.Model model)
      {
         LoadModuleCommand = new RelayCommand<ModuleBaseViewModel>(LoadModule, CanLoadModule);
         LoadFilteredModuleCommand = new RelayCommand<object>(LoadFilteredModule, CanLoadFilteredModule);

         Model = model;
         ModuleBaseViewModels = new ObservableCollection<ModuleBaseViewModel>();
         QuickNavigationModuleViewModels = new ObservableCollection<ModuleBaseViewModel>();
      }

      #endregion

      #region Methods

      private bool CanLoadModule(ModuleBaseViewModel moduleBaseViewModel)
      {
         return moduleBaseViewModel != null;
      }

      private void LoadModule(ModuleBaseViewModel moduleBaseViewModel)
      {
         LoadModule(moduleBaseViewModel, null);
      }

      private bool CanLoadFilteredModule(object parameter)
      {
         var values = (object[])parameter;
         return (values[0] as ModuleBaseViewModel) != null;
      }

      private void LoadFilteredModule(object parameter)
      {
         var values = (object[])parameter;
         var moduleToLoad = values[0] as ModuleBaseViewModel;
         var filters = values[1] as List<string>;

         LoadModule(moduleToLoad, filters);
      }

      private void LoadModule(ModuleBaseViewModel moduleBaseViewModel, List<string> filters)
      {
         if (Model.CanLoadModule(moduleBaseViewModel.ModuleBase))
         {
            Model.LoadModule(moduleBaseViewModel.ModuleBase, filters);
            moduleBaseViewModel.Load();

            LoadedModuleBaseViewModel = moduleBaseViewModel;
         }
      }

      public void Dispose()
      {
         foreach (var viewmModel in ModuleBaseViewModels)
         {
            viewmModel.Dispose();

            if (viewmModel is ModuleGroupViewModel)
            {
               var moduleGroupViewModel = viewmModel as ModuleGroupViewModel;

               foreach (var moduleViewModel in moduleGroupViewModel.ModuleViewModels)
               {
                  moduleViewModel.Dispose();
               }
            }
         }

         ModuleBaseViewModels.Clear();
      }

      #endregion
   }
}
