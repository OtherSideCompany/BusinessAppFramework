using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace OtherSideCore.Wpf.UserControls
{
   /// <summary>
   /// Interaction logic for MultiTextFilterView.xaml
   /// </summary>
   public partial class MultiTextFilterView : UserControl
   {
      public static readonly DependencyProperty MultiTextFilterViewButton_ImageColorProperty =
        DependencyProperty.Register("MultiTextFilterViewButton_ImageColor", typeof(SolidColorBrush), typeof(MultiTextFilterView), new UIPropertyMetadata(Brushes.Blue));
      
      public SolidColorBrush MultiTextFilterViewButton_ImageColor
      {
         get { return (SolidColorBrush)GetValue(MultiTextFilterViewButton_ImageColorProperty); }
         set { SetValue(MultiTextFilterViewButton_ImageColorProperty, value); }
      }

      public static readonly DependencyProperty MultiTextFilterViewCheckedButton_ImageColorProperty =
        DependencyProperty.Register("MultiTextFilterViewCheckedButton_ImageColor", typeof(SolidColorBrush), typeof(MultiTextFilterView), new UIPropertyMetadata(Brushes.AliceBlue));

      public SolidColorBrush MultiTextFilterViewCheckedButton_ImageColor
      {
         get { return (SolidColorBrush)GetValue(MultiTextFilterViewButton_ImageColorProperty); }
         set { SetValue(MultiTextFilterViewButton_ImageColorProperty, value); }
      }

      public static readonly DependencyProperty MultiTextFilterViewMouseDownButton_ImageColorProperty =
        DependencyProperty.Register("MultiTextFilterViewMouseDownButton_ImageColor", typeof(SolidColorBrush), typeof(MultiTextFilterView), new UIPropertyMetadata(Brushes.AliceBlue));

      public SolidColorBrush MultiTextFilterViewMouseDownButton_ImageColor
      {
         get { return (SolidColorBrush)GetValue(MultiTextFilterViewButton_ImageColorProperty); }
         set { SetValue(MultiTextFilterViewButton_ImageColorProperty, value); }
      }

      public MultiTextFilterView()
      {
         InitializeComponent();

         ((INotifyCollectionChanged)FiltersItemsControl.Items).CollectionChanged += MultiTextFilterView_CollectionChanged;
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

      private void MultiTextFilterViewUserControl_GotFocus(object sender, RoutedEventArgs e)
      {
         SearchButton.IsDefault = true;
      }

      private void MultiTextFilterViewUserControl_LostFocus(object sender, RoutedEventArgs e)
      {
         SearchButton.IsDefault = false;
      }
   }
}
