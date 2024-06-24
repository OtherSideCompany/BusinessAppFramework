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
         get => m_ParentGroup;
         set => SetProperty(ref m_ParentGroup, value);
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
