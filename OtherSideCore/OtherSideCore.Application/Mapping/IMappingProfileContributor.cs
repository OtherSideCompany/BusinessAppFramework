using AutoMapper;

namespace OtherSideCore.Application.Mapping
{
   public interface IMappingProfileContributor
   {
      void ConfigureMap(Profile profile);
   }
}
