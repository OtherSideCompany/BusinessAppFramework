using AutoMapper;

namespace Application.Mapping
{
   public interface IMappingProfileContributor
   {
      void ConfigureMap(Profile profile);
   }
}
