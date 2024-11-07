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
      public EmptyListIndicator()
      {
         InitializeComponent();
      }
   }
}
