namespace OtherSideCore.Application.Factories
{
   public class TypeBasedFactory
   {
      #region Fields

      private readonly Dictionary<Type, Func<object>> _factories = new();
      private Func<Type, object>? _fallbackFactory = null;

      #endregion

      #region Properties


      #endregion

      #region Commands



      #endregion

      #region Constructor

      public TypeBasedFactory()
      {
         
      }


      #endregion

      #region Public Methods

      public object CreateFromType<T>() where T : class
      {
         return CreateFromType(typeof(T));
      }

      public object CreateFromType(Type type)
      {
         if (_factories.TryGetValue(type, out var factory))
         {
            return factory();
         }

         if (_fallbackFactory != null)
         {
            return _fallbackFactory(type);
         }

         throw new InvalidOperationException($"No factory registered for type {type.Name}");
      }

      public void Register<T>(Func<object> factory) where T : class
      {
         var type = typeof(T);

         if (_factories.ContainsKey(type))
         {
            throw new InvalidOperationException($"Factory already registered for type {type.Name}");
         }

         _factories[type] = () => factory();
      }

      #endregion

      #region Private Methods

      protected void SetFallbackFactory(Func<Type, object> fallbackFactory)
      {
         _fallbackFactory = fallbackFactory;
      }

      #endregion
   }
}
