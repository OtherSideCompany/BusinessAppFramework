using System.Resources;

namespace OtherSideCore.Adapter.Services
{
   public class LocalizationService : ILocalizationService
   {
      #region Fields

      private readonly List<ResourceManager> _resourceManagers;

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public LocalizationService()
      {
         _resourceManagers = new List<ResourceManager>();
      }


      #endregion

      #region Public Methods

      public string GetString(string key)
      {
         foreach (var manager in _resourceManagers)
         {
            var result = manager.GetString(key);

            if (!string.IsNullOrEmpty(result))
            {
               return result;
            }
         }

         return key;
      }

      public void RegisterResourceManager(ResourceManager resourceManager)
      {
         _resourceManagers.Add(resourceManager);
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
