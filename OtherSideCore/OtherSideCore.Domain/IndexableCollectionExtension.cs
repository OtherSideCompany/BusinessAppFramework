using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Domain
{
   public static class IndexableCollectionExtension
   {
      #region Fields



      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor



      #endregion

      #region Public Methods

      public static void Reindex(List<IIndexable> indexableCollection)
      {
         var index = 0;
         foreach (var item in indexableCollection)
         {
            item.Index = index++;
         }
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
