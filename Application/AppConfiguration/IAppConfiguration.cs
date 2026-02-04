namespace Application.AppConfiguration
{
   public interface IAppConfiguration
   {
      string ConfigFilePath { get; }
      bool RememberUserName { get; set; }
      string UserLogin { get; set; }

      void Load();
      void Save();
   }
}
