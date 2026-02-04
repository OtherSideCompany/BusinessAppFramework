using AutoMapper;
using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Domain.DomainObjects;

namespace BusinessAppFramework.Infrastructure.Mapping
{
   public static class AutoMapperExtensions
   {
      public static IMappingExpression<TSource, TDestination> MapHisytoryInfo<TSource, TDestination>(this IMappingExpression<TSource, TDestination> mapping) where TSource : IEntity
                                                                                                                                                             where TDestination : DomainObject
      {
         return mapping
           .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.HistoryInfo.CreationDate))
           .ForMember(dest => dest.CreatedById, opt => opt.MapFrom(src => src.HistoryInfo.CreatedById))
           .ForMember(dest => dest.CreatedByName, opt => opt.MapFrom(src => src.HistoryInfo.CreatedByName))
           .ForMember(dest => dest.LastModifiedDateTime, opt => opt.MapFrom(src => src.HistoryInfo.LastModifiedDateTime))
           .ForMember(dest => dest.LastModifiedById, opt => opt.MapFrom(src => src.HistoryInfo.LastModifiedById))
           .ForMember(dest => dest.LastModifiedByName, opt => opt.MapFrom(src => src.HistoryInfo.LastModifiedByName));
      }
   }
}
