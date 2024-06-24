using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;

namespace OtherSideCore.Model
{
   public abstract class ModuleBase : ObservableObject, IDisposable
   {
      #region Fields

      private bool m_IsLoaded;
      private string m_Name;
      private string m_ModuleNavigationPath;

      #endregion

      #region Properties

      public bool IsLoaded
      {
         get => m_IsLoaded;
         set => SetProperty(ref m_IsLoaded, value);
      }

      public string Name
      {
         get => m_Name;
         set => SetProperty(ref m_Name, value);
      }

      public string ModuleNavigationPath
      {
         get => m_ModuleNavigationPath;
         set => SetProperty(ref m_ModuleNavigationPath, value);
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public ModuleBase(string name)
      {
         Name = name;
      }

      #endregion

      #region Methods

      public void Load(List<string> filters = null)
      {
         IsLoaded = true;
      }

      public void Unload()
      {
         IsLoaded = false;
      }

      public virtual void Dispose()
      {
         Unload();
      }

      #endregion
   }
}
