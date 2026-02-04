using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace OtherSideCore.Wpf.CustomControls
{
   public class Switch : ToggleButton
   {
      public static readonly DependencyProperty Switch_CheckedTextProperty =
        DependencyProperty.Register("Switch_CheckedTextProperty", typeof(string), typeof(Switch), new UIPropertyMetadata(""));

      public string Switch_CheckedText
      {
         get { return (string)GetValue(Switch_CheckedTextProperty); }
         set { SetValue(Switch_CheckedTextProperty, value); }
      }

      public static readonly DependencyProperty Switch_UncheckedTextProperty =
        DependencyProperty.Register("Switch_UncheckedTextProperty", typeof(string), typeof(Switch), new UIPropertyMetadata(""));

      public string Switch_UncheckedText
      {
         get { return (string)GetValue(Switch_UncheckedTextProperty); }
         set { SetValue(Switch_UncheckedTextProperty, value); }
      }

      public static readonly DependencyProperty Switch_CheckedBackgroundColorProperty =
        DependencyProperty.Register("Switch_CheckedBackgroundColor", typeof(SolidColorBrush), typeof(Switch), new UIPropertyMetadata(Brushes.Transparent));

      public SolidColorBrush Switch_CheckedBackgroundColor
      {
         get { return (SolidColorBrush)GetValue(Switch_CheckedBackgroundColorProperty); }
         set { SetValue(Switch_CheckedBackgroundColorProperty, value); }
      }

      public static readonly DependencyProperty Switch_UncheckedBackgroundColorProperty =
        DependencyProperty.Register("Switch_UncheckedBackgroundColor", typeof(SolidColorBrush), typeof(Switch), new UIPropertyMetadata(Brushes.Transparent));

      public SolidColorBrush Switch_UncheckedBackgroundColor
      {
         get { return (SolidColorBrush)GetValue(Switch_UncheckedBackgroundColorProperty); }
         set { SetValue(Switch_UncheckedBackgroundColorProperty, value); }
      }
   }
}
