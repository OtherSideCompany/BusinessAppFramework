using OtherSideCore.Utils;

namespace OtherSideCore.Model
{
   public abstract class ModuleBase : ObservableObject
   {
      #region Fields

      private bool m_IsLoaded;
      private string m_Name;
      private string m_ModuleNavigationPath;

      #endregion

      #region Properties

      public bool IsLoaded
      {
         get
         {
            return m_IsLoaded;
         }
         private set
         {
            if (value != m_IsLoaded)
            {
               m_IsLoaded = value;
               OnPropertyChanged("IsLoaded");
            }
         }
      }

      public string Name
      {
         get
         {
            return m_Name;
         }
         protected set
         {
            if (value != m_Name)
            {
               m_Name = value;
               OnPropertyChanged("Name");
            }
         }
      }

      public string ModuleNavigationPath
      {
         get
         {
            return m_ModuleNavigationPath;
         }
         set
         {
            if (value != m_ModuleNavigationPath)
            {
               m_ModuleNavigationPath = value;
               OnPropertyChanged("ModuleNavigationPath");
            }
         }
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

      public void Load()
      {
         IsLoaded = true;
      }

      public void Unload()
      {
         IsLoaded = false;
      }

      #endregion
   }
}
