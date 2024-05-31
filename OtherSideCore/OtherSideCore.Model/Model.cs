using OtherSideCore.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
         get
         {
            return m_ModuleBases;
         }
         set
         {
            if (value != m_ModuleBases)
            {
               m_ModuleBases = value;
               OnPropertyChanged(nameof(ModuleBases));
            }
         }
      }

      public ModuleBase LoadedModule
      {
         get
         {
            return m_LoadedModule;
         }
         private set
         {
            if (value != m_LoadedModule)
            {
               m_LoadedModule = value;
               OnPropertyChanged(nameof(LoadedModule));
            }
         }
      }

      public User AuthenticatedUser
      {
         get
         {
            return m_AuthenticatedUser;
         }
         private set
         {
            if (value != m_AuthenticatedUser)
            {
               m_AuthenticatedUser = value;
               OnPropertyChanged(nameof(AuthenticatedUser));
            }
         }
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

      private void UnloadLoadedModule()
      {
         if (LoadedModule != null)
         {
            LoadedModule.Unload();
            LoadedModule = null;
         }
      }

      public void LoadModule(ModuleBase moduleBase, List<string> filters)
      {
         UnloadLoadedModule();

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

      public void Dispose()
      {
         UnloadLoadedModule();
      }

      #endregion
   }
}
