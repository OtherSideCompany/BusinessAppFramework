using AutoMapper;
using OtherSideCore.Domain;
using OtherSideCore.Domain.DomainObjects;
using OtherSideCore.Infrastructure.Entities;
using System.Collections.Generic;

namespace OtherSideCore.Infrastructure.Mapping
{
   public class InfrastructureMappingProfile : Profile
   {
      public InfrastructureMappingProfile()
      {
         CreateMap<Entities.User, Domain.DomainObjects.User>().ReverseMap();

         // Warning : for each IIndexable, use .IgnoreAllCollections().ReverseMap().IgnoreAllVirtual().AfterMap((src, dest) => dest.Index = src.Index)
         // because automapper seems to ignore interface defined properties
      }
   }
}
