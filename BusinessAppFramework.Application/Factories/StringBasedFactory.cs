using BusinessAppFramework.Domain;

namespace BusinessAppFramework.Application.Factories
{
    public class StringBasedFactory
    {
        #region Fields

        protected readonly Dictionary<string, Func<object[], object>> _factories = new();
        protected Func<string, object[], object>? _fallbackFactory = null;

        #endregion

        #region Properties



        #endregion

        #region Commands



        #endregion

        #region Constructor

        public StringBasedFactory()
        {

        }

        #endregion

        #region Public Methods

        public object Create(string key, params object[] args)
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

        public void Register(string key, Func<object> factory)
        {
            Register(key, _ => factory());
        }

        public void Register(string key, Func<object[], object> factory)
        {
            if (_factories.ContainsKey(key))
            {
                throw new InvalidOperationException($"Factory already registered for key {key}.");
            }

            _factories[key] = factory;
        }

        public void SetFallbackFactory(Func<string, object> fallbackFactory)
        {
            _fallbackFactory = (key, args) => fallbackFactory(key);
        }

        public void SetFallbackFactory(Func<string, object[], object> fallbackFactory)
        {
            _fallbackFactory = fallbackFactory;
        }

        public IEnumerable<object> CreateAll(params object[] args)
        {
            return _factories.Values.Select(factory => factory(args));
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
