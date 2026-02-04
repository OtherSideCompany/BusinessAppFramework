namespace BusinessAppFramework.Application
{
   public class ReferenceMap
   {
      #region Fields

      protected readonly Dictionary<Type, List<Type>> _referenceMap;

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public ReferenceMap()
      {
         _referenceMap = new Dictionary<Type, List<Type>>();
      }

      #endregion

      #region Public Methods

      public IEnumerable<Type> GetReferenceTypes(Type sourceType)
      {
         return _referenceMap.ContainsKey(sourceType) ? _referenceMap[sourceType] : Enumerable.Empty<Type>();
      }

      #endregion

      #region Private Methods

      protected void AddReference(Type sourceType, params Type[] targetTypes)
      {
         if (!_referenceMap.ContainsKey(sourceType))
         {
            _referenceMap[sourceType] = new List<Type>();
         }

         foreach (var targetType in targetTypes)
         {
            if (!_referenceMap[sourceType].Contains(targetType))
            {
               _referenceMap[sourceType].Add(targetType);
            }
         }
      }

      #endregion
   }
}
