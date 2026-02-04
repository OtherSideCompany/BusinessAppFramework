using AutoMapper;

namespace BusinessAppFramework.Application.Mapping
{
   public interface IMappingProfileContributor
   {
      void ConfigureMap(Profile profile);
   }
}
