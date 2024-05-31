using OtherSideCore.Model;
using OtherSideCore.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace OtherSideCore.ViewModel
{
   public class ModelViewModel : ObservableObject, IDisposable
   {
      #region Fields

      private Model.Model m_Model;

      private ModuleBaseViewModel m_LoadedModuleBaseViewModel;

      private ObservableCollection<ModuleBaseViewModel> m_ModuleBaseViewModels;
      private ObservableCollection<ModuleBaseViewModel> m_QuickNavigationModuleViewModels;

      private Command m_LoadModuleCommand;
      private Command m_LoadFilteredModuleCommand;

      #endregion

      #region Properties

      public Model.Model Model
      {
         get
         {
            return m_Model;
         }
         set
         {
            if (value != m_Model)
            {
               m_Model = value;
               OnPropertyChanged(nameof(Model));
            }
         }
      }

      public ModuleBaseViewModel LoadedModuleBaseViewModel
      {
         get
         {
            return m_LoadedModuleBaseViewModel;
         }
         set
         {
            if (value != m_LoadedModuleBaseViewModel)
            {
               m_LoadedModuleBaseViewModel = value;
               OnPropertyChanged(nameof(LoadedModuleBaseViewModel));
            }
         }
      }

      public ObservableCollection<ModuleBaseViewModel> ModuleBaseViewModels
      {
         get
         {
            return m_ModuleBaseViewModels;
         }
         set
         {
            if (value != m_ModuleBaseViewModels)
            {
               m_ModuleBaseViewModels = value;
               OnPropertyChanged(nameof(ModuleBaseViewModels));
            }
         }
      }

      public ObservableCollection<ModuleBaseViewModel> QuickNavigationModuleViewModels
      {
         get
         {
            return m_QuickNavigationModuleViewModels;
         }
         set
         {
            if (value != m_QuickNavigationModuleViewModels)
            {
               m_QuickNavigationModuleViewModels = value;
               OnPropertyChanged(nameof(QuickNavigationModuleViewModels));
            }
         }
      }

      #endregion

      #region Commands

      public Command LoadModuleCommand
      {
         get
         {
            if (m_LoadModuleCommand == null)
            {
               m_LoadModuleCommand = new Command(ExecuteLoadModuleCommand, CanExecuteLoadModuleCommand);
            }
            return m_LoadModuleCommand;
         }
      }

      public Command LoadFilteredModuleCommand
      {
         get
         {
            if (m_LoadFilteredModuleCommand == null)
            {
               m_LoadFilteredModuleCommand = new Command(ExecuteLoadFilteredModuleCommand, CanExecuteLoadFilteredModuleCommand);
            }
            return m_LoadFilteredModuleCommand;
         }
      }

      #endregion

      #region Constructor

      public ModelViewModel(Model.Model model)
      {
         Model = model;
         ModuleBaseViewModels = new ObservableCollection<ModuleBaseViewModel>();
         QuickNavigationModuleViewModels = new ObservableCollection<ModuleBaseViewModel>();
      }

      #endregion

      #region Methods

      private bool CanExecuteLoadModuleCommand(object parameter)
      {
         return CanLoadModule(parameter as ModuleBaseViewModel);
      }

      private void ExecuteLoadModuleCommand(object parameter)
      {
         LoadModule(parameter as ModuleBaseViewModel, null);
      }

      private bool CanExecuteLoadFilteredModuleCommand(object parameter)
      {
         var values = (object[])parameter;
         return CanLoadModule(values[0] as ModuleBaseViewModel);
      }

      private void ExecuteLoadFilteredModuleCommand(object parameter)
      {
         var values = (object[])parameter;
         var moduleToLoad = values[0] as ModuleBaseViewModel;
         var filters = values[1] as List<string>;

         LoadModule(moduleToLoad, filters);
      }

      private bool CanLoadModule(ModuleBaseViewModel moduleBaseViewModel)
      {
         return moduleBaseViewModel != null;
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
