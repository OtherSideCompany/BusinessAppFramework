using CommunityToolkit.Mvvm.ComponentModel;
using OtherSideCore.Model.ModelObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace OtherSideCore.Model
{
   public abstract class Model : ObservableObject, IDisposable
   {
      #region Fields

      private ModuleBase m_LoadedModule;
      private ObservableCollection<ModuleBase> m_ModuleBases;
      private User m_AuthenticatedUser;

      #endregion

      #region Properties

      public ObservableCollection<ModuleBase> ModuleBases
      {
         get => m_ModuleBases;
         set => SetProperty(ref m_ModuleBases, value);
      }

      public ModuleBase LoadedModule
      {
         get => m_LoadedModule;
         set => SetProperty(ref m_LoadedModule, value);
      }

      public User AuthenticatedUser
      {
         get => m_AuthenticatedUser;
         set => SetProperty(ref m_AuthenticatedUser, value);
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public Model(User authenticatedUser)
      {
         ModuleBases = new ObservableCollection<ModuleBase>();
         AuthenticatedUser = authenticatedUser;
      }

      #endregion

      #region Methods

      public abstract void InitializeModules();

      public bool CanLoadModule(ModuleBase moduleBase)
      {
         return moduleBase != LoadedModule && !moduleBase.IsLoaded && AuthenticatedUser != null;
      }

      public void LoadModule(ModuleBase moduleBase, List<string> filters)
      {
         UnloadLoadModule();

         if (moduleBase != null)
         {
            if (filters != null && filters.Any())
            {
               moduleBase.Load(filters);
            }
            else
            {
               moduleBase.Load();
            }

            LoadedModule = moduleBase;
         }
      }

      private void UnloadLoadModule()
      {
         if (LoadedModule != null)
         {
            LoadedModule.Unload();
            LoadedModule = null;
         }
      }

      public void Dispose()
      {
         UnloadLoadModule();
      }

      #endregion
   }
}
