using AutoMapper;
using OtherSideCore.Domain.DomainObjects;
using OtherSideCore.Infrastructure.Entities;

namespace OtherSideCore.Infrastructure.Mapping
{
   public class InfrastructureMappingProfile : Profile
   {
      public InfrastructureMappingProfile()
      {
         CreateMap<EntityBase, DomainObject>()
            .IncludeAllDerived()
            .ReverseMap()
            .ForMember(dest => dest.LastModifiedBy, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .IncludeAllDerived();

         CreateMap<Entities.User, Domain.DomainObjects.User>().ReverseMap();
         
      }
   }
}
