using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Adapter
{
   public class TextFilterViewModel : ObservableObject
   {
      #region Fields

      private string _filterText;

      #endregion

      #region Properties

      public string FilterText
      {
         get => _filterText;
         set => SetProperty(ref _filterText, value);
      }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public TextFilterViewModel(string filterText)
      {
         FilterText = filterText;
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
