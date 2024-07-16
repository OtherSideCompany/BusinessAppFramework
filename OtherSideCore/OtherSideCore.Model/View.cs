using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Model
{
   public class View : ViewBase
   {
      #region Fields

      private ViewGroup _parentViewGroup;

      #endregion

      #region Properties

      public ViewGroup ParentViewGroup
      {
         get => _parentViewGroup;
         set => SetProperty(ref _parentViewGroup, value);
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public View(string name, Type viewModelType, object iconResource, ViewGroup parent = null) : base(name, viewModelType, iconResource)
      {
         ParentViewGroup = parent;

         if (ParentViewGroup != null)
         {
            ViewNavigationPath = parent.Name + " > " + Name;
         }
         else
         {
            ViewNavigationPath = Name;
         }
      }

      #endregion

      #region Methods



      #endregion
   }
}
