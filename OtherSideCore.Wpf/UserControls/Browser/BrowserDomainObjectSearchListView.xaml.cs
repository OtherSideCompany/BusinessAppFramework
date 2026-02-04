using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OtherSideCore.Wpf.UserControls.Browser
{
   /// <summary>
   /// Interaction logic for BrowserDomainObjectSearchListView.xaml
   /// </summary>
   public partial class BrowserDomainObjectSearchListView : UserControl
   {
      public static readonly DependencyProperty BrowserDomainObjectSearchListView_GridViewProperty =
          DependencyProperty.Register("BrowserDomainObjectSearchListView_GridView", typeof(GridView), typeof(BrowserDomainObjectSearchListView), new UIPropertyMetadata());

      public GridView BrowserDomainObjectSearchListView_GridView
      {
         get { return (GridView)GetValue(BrowserDomainObjectSearchListView_GridViewProperty); }
         set { SetValue(BrowserDomainObjectSearchListView_GridViewProperty, value); }
      }

      public static readonly DependencyProperty BrowserDomainObjectSearchListView_ThumbColorProperty =
          DependencyProperty.Register("BrowserDomainObjectSearchListView_ThumbColor", typeof(SolidColorBrush), typeof(BrowserDomainObjectSearchListView), new UIPropertyMetadata(Brushes.Gray));

      public SolidColorBrush BrowserDomainObjectSearchListView_ThumbColor
      {
         get { return (SolidColorBrush)GetValue(BrowserDomainObjectSearchListView_ThumbColorProperty); }
         set { SetValue(BrowserDomainObjectSearchListView_ThumbColorProperty, value); }
      }

      public static readonly DependencyProperty BrowserDomainObjectSearchListView_SelectedItemBackgroundColorProperty =
          DependencyProperty.Register("BrowserDomainObjectSearchListView_SelectedItemBackgroundColor", typeof(SolidColorBrush), typeof(BrowserDomainObjectSearchListView), new UIPropertyMetadata(Brushes.Blue));

      public SolidColorBrush BrowserDomainObjectSearchListView_SelectedItemBackgroundColor
      {
         get { return (SolidColorBrush)GetValue(BrowserDomainObjectSearchListView_SelectedItemBackgroundColorProperty); }
         set { SetValue(BrowserDomainObjectSearchListView_SelectedItemBackgroundColorProperty, value); }
      }

      public static readonly DependencyProperty BrowserDomainObjectSearchListView_HeaderForegroundColorProperty =
          DependencyProperty.Register("BrowserDomainObjectSearchListView_HeaderForegroundColor", typeof(SolidColorBrush), typeof(BrowserDomainObjectSearchListView), new UIPropertyMetadata(Brushes.Yellow));

      public SolidColorBrush BrowserDomainObjectSearchListView_HeaderForegroundColor
      {
         get { return (SolidColorBrush)GetValue(BrowserDomainObjectSearchListView_HeaderForegroundColorProperty); }
         set { SetValue(BrowserDomainObjectSearchListView_HeaderForegroundColorProperty, value); }
      }

      public BrowserDomainObjectSearchListView()
      {
         InitializeComponent();
      }
   }
}
