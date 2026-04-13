namespace BusinessAppFramework.Application.Interfaces
{
   public interface ILocalizedStringService
   {
      void Add(string key, string culture, string value);
      string Get(string key);
      string Get(string key, string culture);
   }
}
