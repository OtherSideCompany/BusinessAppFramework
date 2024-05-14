using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Model
{
   public class Module : ModuleBase
   {
      #region Fields

      private ModuleGroup m_ParentGroup;

      #endregion

      #region Properties

      public ModuleGroup ParentGroup
      {
         get
         {
            return m_ParentGroup;
         }
         private set
         {
            if (value != m_ParentGroup)
            {
               m_ParentGroup = value;
               OnPropertyChanged("ParentGroup");
            }
         }
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public Module(string name, ModuleGroup parent) : base(name)
      {
         ParentGroup = parent;

         if (ParentGroup != null)
         {
            ModuleNavigationPath = parent.Name + " > " + Name;
         }
         else
         {
            ModuleNavigationPath = Name;
         }

      }

      #endregion

      #region Methods



      #endregion
   }
}
