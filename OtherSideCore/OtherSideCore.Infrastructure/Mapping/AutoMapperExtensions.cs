using AutoMapper;
using System.Collections;
using System.Linq;

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

      public static IMappingExpression<TSource, TDestination> IgnoreAllCollections<TSource, TDestination>(this IMappingExpression<TSource, TDestination> mapping)
      {
         var collectionProperties = typeof(TDestination).GetProperties().Where(p => p.PropertyType != typeof(string) &&
                                                                               typeof(IEnumerable).IsAssignableFrom(p.PropertyType));

         foreach (var property in collectionProperties)
         {
            mapping.ForMember(property.Name, opt => opt.Ignore());
         }

         return mapping;
      }
   }
}
