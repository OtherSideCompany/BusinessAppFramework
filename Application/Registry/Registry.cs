namespace Application.Registry
{
   public class Registry<TKey, TValue> where TKey : notnull
   {
      protected readonly Dictionary<TKey, TValue> _items;

      public IEnumerable<TKey> Keys => _items.Keys;
      public IEnumerable<TValue> Values => _items.Values;

      protected Registry()
      {
         _items = new Dictionary<TKey, TValue>();
      }

      public void Register(TKey key, TValue value)
      {
         _items[key] = value;
      }

      public TValue Resolve(TKey key)
      {
         if (_items.TryGetValue(key, out var value))
            return value;

         throw new KeyNotFoundException($"No value registered for key '{key}'.");
      }

      public bool TryResolve(TKey key, out TValue value)
      {
         return _items.TryGetValue(key, out value!);
      }


   }
}
