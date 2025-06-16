using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Application.Factories
{
   public class EnumBasedFactory<TEnum> where TEnum : struct, Enum
   {
      #region Fields

      private readonly Dictionary<TEnum, Func<object>> _factories = new();
      private Func<TEnum, object>? _fallbackFactory = null;

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public EnumBasedFactory()
      {

      }

      #endregion

      #region Public Methods

      public object Create(TEnum key)
      {
         if (_factories.TryGetValue(key, out var factory))
         {
            return factory();
         }

         if (_fallbackFactory != null)
         {
            return _fallbackFactory(key);
         }

         throw new InvalidOperationException($"No factory registered for key {key}.");
      }

      public void Register(TEnum key, Func<object> factory)
      {
         if (_factories.ContainsKey(key))
         {
            throw new InvalidOperationException($"Factory already registered for key {key}.");
         }

         _factories[key] = factory;
      }

      public void SetFallbackFactory(Func<TEnum, object> fallbackFactory)
      {
         _fallbackFactory = fallbackFactory;
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
