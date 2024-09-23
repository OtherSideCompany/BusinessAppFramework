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
      }

      private void SearchBorder_GotFocus(object sender, RoutedEventArgs e)
      {
         ResetIsDefaultState();
         SearchButton.IsDefault = true;
      }

      private void SearchBorder_LostFocus(object sender, RoutedEventArgs e)
      {
         ResetIsDefaultState();
      }

      private void FiltersBorder_GotFocus(object sender, RoutedEventArgs e)
      {
         ResetIsDefaultState();
         RefreshButton.IsDefault = true;
      }

      private void FiltersBorder_LostFocus(object sender, RoutedEventArgs e)
      {
         ResetIsDefaultState();
      }

      private void ResetIsDefaultState()
      {
         SearchButton.IsDefault = false;
         RefreshButton.IsDefault = false;
      }
   }
}
