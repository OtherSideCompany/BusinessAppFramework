using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace OtherSideCore.Wpf.CustomControls
{
   public class TextToggleButton : ToggleButton
   {    
      public static readonly DependencyProperty TextToggleButton_OverBackgroundColorProperty =
        DependencyProperty.Register("TextToggleButton_OverBackgroundColor", typeof(SolidColorBrush), typeof(TextToggleButton), new UIPropertyMetadata(Brushes.Transparent));

      public SolidColorBrush TextToggleButton_OverBackgroundColor
      {
         get { return (SolidColorBrush)GetValue(TextToggleButton_OverBackgroundColorProperty); }
         set { SetValue(TextToggleButton_OverBackgroundColorProperty, value); }
      }

      public static readonly DependencyProperty TextToggleButton_MouseDownBackgroundColorProperty =
        DependencyProperty.Register("TextToggleButton_MouseDownBackgroundColor", typeof(SolidColorBrush), typeof(TextToggleButton), new UIPropertyMetadata(Brushes.Transparent));

      public SolidColorBrush TextToggleButton_MouseDownBackgroundColor
      {
         get { return (SolidColorBrush)GetValue(TextToggleButton_MouseDownBackgroundColorProperty); }
         set { SetValue(TextToggleButton_MouseDownBackgroundColorProperty, value); }
      }

      public static readonly DependencyProperty TextToggleButton_CheckedBackgroundColorProperty =
        DependencyProperty.Register("TextToggleButton_CheckedBackgroundColor", typeof(SolidColorBrush), typeof(TextToggleButton), new UIPropertyMetadata(Brushes.Transparent));

      public SolidColorBrush TextToggleButton_CheckedBackgroundColor
      {
         get { return (SolidColorBrush)GetValue(TextToggleButton_CheckedBackgroundColorProperty); }
         set { SetValue(TextToggleButton_CheckedBackgroundColorProperty, value); }
      }

      static TextToggleButton()
      {
         DefaultStyleKeyProperty.OverrideMetadata(typeof(TextToggleButton), new FrameworkPropertyMetadata(typeof(TextToggleButton)));
      }
   }
}
