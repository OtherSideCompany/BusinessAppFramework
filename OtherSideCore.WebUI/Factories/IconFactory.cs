using MudBlazor;
using OtherSideCore.Application.Factories;
using OtherSideCore.Domain;
using OtherSideCore.WebUI.Interfaces;

namespace OtherSideCore.WebUI.Factories
{
    public class IconFactory : StringKeyBasedFactory, IIconFactory
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

        public string CreateIcon(StringKey key)
        {
            return (string)Create(key);
        }

        public void RegisterIcon(StringKey key, string icon)
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
