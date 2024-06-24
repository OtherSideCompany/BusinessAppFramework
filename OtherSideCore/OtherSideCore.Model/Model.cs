using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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

      public Model()
      {
         ModuleBases = new ObservableCollection<ModuleBase>();
      }

      #endregion

      #region Methods

      public abstract void InitializeModules();

      public bool CanLoadModule(ModuleBase moduleBase)
      {
         return moduleBase != LoadedModule && !moduleBase.IsLoaded;
      }

      private void UnloadLoadModule()
      {
         if (LoadedModule != null)
         {
            LoadedModule.Unload();
            LoadedModule = null;
         }
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

      public bool CanAuthenticateUser()
      {
         return AuthenticatedUser == null;
      }

      public bool AuthenticateUser(User user, string passwordHash)
      {
         if (CanAuthenticateUser())
         {
            if (user.Authenticate(passwordHash))
            {
               AuthenticatedUser = user;
            }
         }

         return false;
      }

      public void DisconnectAuthenticatedUser()
      {
         UnloadLoadModule();
         AuthenticatedUser = null;
      }

      public void Dispose()
      {
         UnloadLoadModule();
      }

      #endregion
   }
}
