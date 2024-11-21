using System.Windows;
using System.Windows.Controls;
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


      public MultiTextFilterView()
      {
         InitializeComponent();
      }      
   }
}
