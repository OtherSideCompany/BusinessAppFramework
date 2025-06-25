using AutoMapper;
using OtherSideCore.Adapter;
using OtherSideCore.Domain.Attributes;
using OtherSideCore.Domain.DomainObjects;
using System.Linq;
using System.Reflection;

namespace OtherSideCore.Infrastructure.Mapping
{
   public static class AutoMapperExtensions
   {
      public static IMappingExpression<TSource, TDestination> IgnoreAllVirtual<TSource, TDestination>(this IMappingExpression<TSource, TDestination> mapping)
      {
         var virtualProperties = typeof(TDestination).GetProperties().Where(p => p.GetMethod != null && p.GetMethod.IsVirtual);

         foreach (var property in virtualProperties)
         {
            mapping.ForMember(property.Name, opt => opt.Ignore());
         }

         return mapping;
      }

      public static IMappingExpression<TSource, TDestination> IgnoreCollectionsMapping<TSource, TDestination>(this IMappingExpression<TSource, TDestination> mapping)
      {
         var taggedCollectionProperties = typeof(TDestination).GetProperties()
                                                              .Where(p => p.GetCustomAttribute<IgnoreCollectionMapping>() != null);

         foreach (var property in taggedCollectionProperties)
         {
            mapping.ForMember(property.Name, opt => opt.Ignore());
         }

         return mapping;
      }

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
