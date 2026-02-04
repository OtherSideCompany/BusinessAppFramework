using BusinessAppFramework.Domain;

namespace BusinessAppFramework.Application.Factories
{
   public class StringKeyBasedFactory
   {
      #region Fields

      protected readonly Dictionary<StringKey, Func<object[], object>> _factories = new();
      protected Func<StringKey, object[], object>? _fallbackFactory = null;

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public StringKeyBasedFactory()
      {

      }

      #endregion

      #region Public Methods

      public object Create(StringKey key, params object[] args)
      {
         if (_factories.TryGetValue(key, out var factory))
         {
            return factory(args);
         }

         if (_fallbackFactory != null)
         {
            return _fallbackFactory(key, args);
         }

         throw new InvalidOperationException($"No factory registered for key {key}.");
      }

      public void Register(StringKey key, Func<object> factory)
      {
         Register(key, _ => factory());
      }

      public void Register(StringKey key, Func<object[], object> factory)
      {
         if (_factories.ContainsKey(key))
         {
            throw new InvalidOperationException($"Factory already registered for key {key}.");
         }

         _factories[key] = factory;
      }

      public void SetFallbackFactory(Func<StringKey, object> fallbackFactory)
      {
         _fallbackFactory = (key, args) => fallbackFactory(key);
      }

      public void SetFallbackFactory(Func<StringKey, object[], object> fallbackFactory)
      {
         _fallbackFactory = fallbackFactory;
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
