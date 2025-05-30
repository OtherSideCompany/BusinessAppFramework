using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace OtherSideCore.Bootstrapper
{
   public abstract class Module : IModule
   {
      #region Fields



      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public Module()
      {

      }

      #endregion

      #region Public Methods

      public abstract void RegisterServices(IServiceCollection services);

      #endregion

      #region Private Methods

      protected void AddAutoMapperProfiles(IServiceCollection services, Type infrastructureProfileType, Type adapterProfileType)
      {
         services.AddAutoMapper(cfg =>
         {
            cfg.AddProfile(infrastructureProfileType);
            cfg.AddProfile(adapterProfileType);
         });

#if DEBUG
         try
         {
            var configuration = new MapperConfiguration(cfg => { cfg.AddProfile(infrastructureProfileType); });
            configuration.AssertConfigurationIsValid();

            configuration = new MapperConfiguration(cfg => { cfg.AddProfile(adapterProfileType); });
            configuration.AssertConfigurationIsValid();
         }
         catch (AutoMapperConfigurationException ex)
         {
            System.Diagnostics.Debug.WriteLine($"AutoMapper configuration is invalid: {ex.Message}");
         }
#endif
      }

      #endregion
   }
}
