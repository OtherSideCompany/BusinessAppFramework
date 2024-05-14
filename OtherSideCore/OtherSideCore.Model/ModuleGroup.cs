using System.Collections.ObjectModel;

namespace OtherSideCore.Model
{
   public class ModuleGroup : ModuleBase
   {
      #region Fields

      private ObservableCollection<Module> m_Modules;

      #endregion

      #region Properties

      public ObservableCollection<Module> Modules
      {
         get
         {
            return m_Modules;
         }
         private set
         {
            if (value != m_Modules)
            {
               m_Modules = value;
               OnPropertyChanged("Modules");
            }
         }
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public ModuleGroup(string name) : base(name)
      {
         Modules = new ObservableCollection<Module>();
         ModuleNavigationPath = Name;
      }

      #endregion

      #region Methods



      #endregion
   }
}
