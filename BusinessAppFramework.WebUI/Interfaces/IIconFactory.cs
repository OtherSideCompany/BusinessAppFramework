using BusinessAppFramework.Domain;

namespace BusinessAppFramework.WebUI.Interfaces
{
   public interface IIconFactory
   {
      void RegisterIcon(string key, string icon);
      string CreateIcon(string key);
   }
}
