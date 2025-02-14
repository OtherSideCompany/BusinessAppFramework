using OtherSideCore.Adapter.DomainObjectInteraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OtherSideCore.Wpf.UserControls.Filters
{
   /// <summary>
   /// Interaction logic for TextFilterView.xaml
   /// </summary>
   public partial class TextFilterView : UserControl
   {
      public TextFilterView()
      {
         InitializeComponent();
      }

      private async void TextFilter_KeyDown(object sender, KeyEventArgs e)
      {
         if (e.Key == Key.Enter)
         {
            if (DataContext is IDomainObjectSearchViewModel domainObjectSearchViewModel)
            {
               await domainObjectSearchViewModel.PaginatedSearchAsync(new PaginatedSearchParameters() { ExtendedSearch = false, ResetPage = true });
            }
         }
      }
   }
}
