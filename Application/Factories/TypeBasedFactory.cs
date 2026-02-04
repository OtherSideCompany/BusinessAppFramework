namespace Application.Factories
{
   public class TypeBasedFactory
   {
      #region Fields

      private readonly Dictionary<Type, Func<object[], object>> _factories = new();
      private Func<Type, object[], object>? _fallbackFactory = null;

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

      public object CreateFromType<T>(params object[] args)
      {
         return CreateFromType(typeof(T), args);
      }

      public object CreateFromType(Type type, params object[] args)
      {
         if (_factories.TryGetValue(type, out var factory))
         {
            return factory(args);
         }

         if (_fallbackFactory != null)
         {
            return _fallbackFactory(type, args);
         }

         throw new InvalidOperationException($"No factory registered for type {type.Name}");
      }

      public void Register<T>(Func<object> factory)
      {
         Register(typeof(T), args => factory());
      }

      public void Register(Type type, Func<object> factory)
      {
         Register(type, args => factory());
      }

      public void Register<T>(Func<object[], object> factory)
      {
         Register(typeof(T), factory);
      }

      public void Register(Type type, Func<object[], object> factory)
      {
         if (_factories.ContainsKey(type))
         {
            throw new InvalidOperationException($"Factory already registered for type {type.Name}");
         }

         _factories[type] = factory;
      }

      public void SetFallbackFactory(Func<Type, object> fallbackFactory)
      {
         _fallbackFactory = (type, args) => fallbackFactory(type);
      }

      public void SetFallbackFactory(Func<Type, object[], object> fallbackFactory)
      {
         _fallbackFactory = fallbackFactory;
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
