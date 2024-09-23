using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OtherSideCore.Wpf.CustomControls
{
   public class TextRadioButton : RadioButton
   {
      public static readonly DependencyProperty TextRadioButton_TextProperty =
        DependencyProperty.Register("TextRadioButton_Text", typeof(string), typeof(TextRadioButton), new UIPropertyMetadata(""));

      public string TextRadioButton_Text
      {
         get { return (string)GetValue(TextRadioButton_TextProperty); }
         set { SetValue(TextRadioButton_TextProperty, value); }
      }

      public static readonly DependencyProperty TextRadioButton_SelectedColorProperty =
        DependencyProperty.Register("TextRadioButton_SelectedColor", typeof(SolidColorBrush), typeof(TextRadioButton), new UIPropertyMetadata(Brushes.Blue));

      public SolidColorBrush TextRadioButton_SelectedColor
      {
         get { return (SolidColorBrush)GetValue(TextRadioButton_SelectedColorProperty); }
         set { SetValue(TextRadioButton_SelectedColorProperty, value); }
      }

      static TextRadioButton()
      {
         DefaultStyleKeyProperty.OverrideMetadata(typeof(TextRadioButton), new FrameworkPropertyMetadata(typeof(TextRadioButton)));
      }
   }
}
