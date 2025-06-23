using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OtherSideCore.Wpf.UserControls
{
   /// <summary>
   /// Interaction logic for Tag.xaml
   /// </summary>
   public partial class Tag : UserControl
   {
      public static readonly DependencyProperty Tag_TextProperty =
          DependencyProperty.Register("Tag_Text", typeof(string), typeof(Tag), new UIPropertyMetadata("-NA-"));

      public string Tag_Text
      {
         get { return (string)GetValue(Tag_TextProperty); }
         set { SetValue(Tag_TextProperty, value); }
      }

      public static readonly DependencyProperty Tag_BackgroundProperty =
          DependencyProperty.Register("Tag_Background", typeof(SolidColorBrush), typeof(Tag), new UIPropertyMetadata(Brushes.Blue));

      public SolidColorBrush Tag_Background
      {
         get { return (SolidColorBrush)GetValue(Tag_BackgroundProperty); }
         set { SetValue(Tag_BackgroundProperty, value); }
      }

      public static readonly DependencyProperty Tag_ForegroundProperty =
          DependencyProperty.Register("Tag_Foreground", typeof(SolidColorBrush), typeof(Tag), new UIPropertyMetadata(Brushes.White));

      public SolidColorBrush Tag_Foreground
      {
         get { return (SolidColorBrush)GetValue(Tag_ForegroundProperty); }
         set { SetValue(Tag_ForegroundProperty, value); }
      }

      public Tag()
      {
         InitializeComponent();
      }
   }
}
