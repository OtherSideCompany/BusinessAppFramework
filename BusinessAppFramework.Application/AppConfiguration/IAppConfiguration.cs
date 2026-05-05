namespace BusinessAppFramework.Application.AppConfiguration
{
   public interface IAppConfiguration
   {
      string ConfigFilePath { get; }
      string UserLogin { get; set; }

      void Load();
      void Save();
   }
}
