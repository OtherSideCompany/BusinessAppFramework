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

namespace OtherSideCore.Wpf.UserControls
{
   /// <summary>
   /// Interaction logic for EmptyObjectIndicator.xaml
   /// </summary>
   public partial class EmptyObjectIndicator : UserControl
   {
      public static readonly DependencyProperty EmptyObjectIndicator_TextProperty =
          DependencyProperty.Register("EmptyObjectIndicator_Text", typeof(string), typeof(EmptyObjectIndicator), new UIPropertyMetadata("Aucun élément associé"));

      public string EmptyObjectIndicator_Text
      {
         get { return (string)GetValue(EmptyObjectIndicator_TextProperty); }
         set { SetValue(EmptyObjectIndicator_TextProperty, value); }
      }

      public static readonly DependencyProperty EmptyObjectIndicator_BorderThicknessProperty =
          DependencyProperty.Register("EmptyObjectIndicator_BorderThickness", typeof(Thickness), typeof(EmptyObjectIndicator), new UIPropertyMetadata(new Thickness(1)));

      public Thickness EmptyObjectIndicator_BorderThickness
      {
         get { return (Thickness)GetValue(EmptyObjectIndicator_BorderThicknessProperty); }
         set { SetValue(EmptyObjectIndicator_BorderThicknessProperty, value); }
      }

      public static readonly DependencyProperty EmptyObjectIndicator_BackgroundColorProperty =
          DependencyProperty.Register("EmptyObjectIndicator_BackgroundColor", typeof(SolidColorBrush), typeof(EmptyObjectIndicator), new UIPropertyMetadata(Brushes.White));

      public SolidColorBrush EmptyObjectIndicator_BackgroundColor
      {
         get { return (SolidColorBrush)GetValue(EmptyObjectIndicator_BackgroundColorProperty); }
         set { SetValue(EmptyObjectIndicator_BackgroundColorProperty, value); }
      }

      public EmptyObjectIndicator()
      {
         InitializeComponent();
      }
   }
}
