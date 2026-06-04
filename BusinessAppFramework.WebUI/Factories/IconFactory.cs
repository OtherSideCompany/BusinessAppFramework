using BusinessAppFramework.Application.Factories;
using BusinessAppFramework.Domain;
using BusinessAppFramework.WebUI.Interfaces;
using MudBlazor;

namespace BusinessAppFramework.WebUI.Factories
{
   public class IconFactory : stringBasedFactory, IIconFactory
   {
      #region Fields



      #endregion

      #region Properties



      #endregion

      #region Events



      #endregion

      #region Constructor

      public IconFactory()
      {
         SetFallbackFactory(key => Icons.Material.Sharp.Cancel);
      }

      public string CreateIcon(string key)
      {
         return (string)Create(key);
      }

      public void RegisterIcon(string key, string icon)
      {
         Register(key, () => icon);
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
