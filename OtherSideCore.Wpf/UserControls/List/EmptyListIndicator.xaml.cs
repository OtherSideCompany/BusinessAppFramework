using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OtherSideCore.Wpf.UserControls.List
{
   /// <summary>
   /// Interaction logic for EmptyListIndicator.xaml
   /// </summary>
   public partial class EmptyListIndicator : UserControl
   {
      public static readonly DependencyProperty EmptyListIndicator_TextProperty =
          DependencyProperty.Register("EmptyListIndicator_Text", typeof(string), typeof(EmptyListIndicator), new UIPropertyMetadata("Aucun élément trouvé"));

      public string EmptyListIndicator_Text
      {
         get { return (string)GetValue(EmptyListIndicator_TextProperty); }
         set { SetValue(EmptyListIndicator_TextProperty, value); }
      }

      public static readonly DependencyProperty EmptyListIndicator_BorderThicknessProperty =
          DependencyProperty.Register("EmptyListIndicator_BorderThickness", typeof(Thickness), typeof(EmptyListIndicator), new UIPropertyMetadata(new Thickness(1)));

      public string EmptyListIndicator_BorderThickness
      {
         get { return (string)GetValue(EmptyListIndicator_BorderThicknessProperty); }
         set { SetValue(EmptyListIndicator_BorderThicknessProperty, value); }
      }

      public static readonly DependencyProperty EmptyListIndicator_TextHorizontalAlignmentProperty =
          DependencyProperty.Register("EmptyListIndicator_TextHorizontalAlignment", typeof(HorizontalAlignment), typeof(EmptyListIndicator), new UIPropertyMetadata(HorizontalAlignment.Left));

      public HorizontalAlignment EmptyListIndicator_TextHorizontalAlignment
      {
         get { return (HorizontalAlignment)GetValue(EmptyListIndicator_TextHorizontalAlignmentProperty); }
         set { SetValue(EmptyListIndicator_TextHorizontalAlignmentProperty, value); }
      }

      public static readonly DependencyProperty EmptyListIndicator_BackgroundProperty =
          DependencyProperty.Register("EmptyListIndicator_Background", typeof(SolidColorBrush), typeof(EmptyListIndicator), new UIPropertyMetadata(Brushes.White));

      public SolidColorBrush EmptyListIndicator_Background
      {
         get { return (SolidColorBrush)GetValue(EmptyListIndicator_BackgroundProperty); }
         set { SetValue(EmptyListIndicator_BackgroundProperty, value); }
      }

      public EmptyListIndicator()
      {
         InitializeComponent();
      }
   }
}
