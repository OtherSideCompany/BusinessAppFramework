using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection.Metadata;
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

namespace OtherSideCore.Wpf.UserControls
{
   /// <summary>
   /// Interaction logic for MultiTextFilterView.xaml
   /// </summary>
   public partial class MultiTextFilterView : UserControl
   {
      public MultiTextFilterView()
      {
         InitializeComponent();

         ((INotifyCollectionChanged)FiltersItemsControl.Items).CollectionChanged += MultiTextFilterView_CollectionChanged;
      }

      private void MultiTextFilterViewUserControl_GotFocus(object sender, RoutedEventArgs e)
      {
         SearchButton.IsDefault = true;
      }

      private void MultiTextFilterViewUserControl_LostFocus(object sender, RoutedEventArgs e)
      {
         SearchButton.IsDefault = false;
      }

      private void MultiTextFilterView_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
      {
         if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems.Count > 0)
         {
            var newItem = e.NewItems[0];
            var lastItem = (ContentPresenter)FiltersItemsControl.ItemContainerGenerator.ContainerFromItem(newItem);

            lastItem.ApplyTemplate();
            var textBox = (TextBox)lastItem.ContentTemplate.FindName("FilterTextBox", lastItem);
            Keyboard.Focus(textBox);
         }
      }
   }
}
