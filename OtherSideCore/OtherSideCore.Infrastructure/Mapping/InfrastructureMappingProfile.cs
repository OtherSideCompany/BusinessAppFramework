using AutoMapper;
using OtherSideCore.Domain.DomainObjects;
using OtherSideCore.Infrastructure.Entities;

namespace OtherSideCore.Infrastructure.Mapping
{
   public class InfrastructureMappingProfile : Profile
   {
      public InfrastructureMappingProfile()
      {
         CreateMap<Entities.User, Domain.DomainObjects.User>().ReverseMap();
      }
   }
}
