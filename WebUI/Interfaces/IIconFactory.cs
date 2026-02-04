using Domain;

namespace WebUI.Interfaces
{
   public interface IIconFactory
   {
      void RegisterIcon(StringKey key, string icon);
      string CreateIcon(StringKey key);
   }
}
