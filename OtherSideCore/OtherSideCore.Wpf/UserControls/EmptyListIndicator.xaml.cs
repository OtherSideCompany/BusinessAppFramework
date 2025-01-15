using System.Windows;
using System.Windows.Controls;

namespace OtherSideCore.Wpf.UserControls
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
      public EmptyListIndicator()
      {
         InitializeComponent();
      }
   }
}
