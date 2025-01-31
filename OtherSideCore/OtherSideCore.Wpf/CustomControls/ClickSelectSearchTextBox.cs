using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OtherSideCore.Wpf.CustomControls
{
   public class ClickSelectSearchTextBox : TextBox
   {
      public static readonly DependencyProperty ClickSelectSearchTextBox_WatermarkProperty =
        DependencyProperty.Register("ClickSelectSearchTextBox_Watermark", typeof(string), typeof(ClickSelectSearchTextBox), new UIPropertyMetadata(String.Empty));

      public string ClickSelectSearchTextBox_Watermark
      {
         get { return (string)GetValue(ClickSelectSearchTextBox_WatermarkProperty); }
         set { SetValue(ClickSelectSearchTextBox_WatermarkProperty, value); }
      }

      public static readonly DependencyProperty ClickSelectSearchTextBox_TextMarginProperty =
        DependencyProperty.Register("ClickSelectSearchTextBox_TextMargin", typeof(Thickness), typeof(ClickSelectSearchTextBox), new UIPropertyMetadata(new Thickness(8, 0, 8, 0)));

      public Thickness ClickSelectSearchTextBox_TextMargin
      {
         get { return (Thickness)GetValue(ClickSelectSearchTextBox_TextMarginProperty); }
         set { SetValue(ClickSelectSearchTextBox_TextMarginProperty, value); }
      }

      public static readonly DependencyProperty ClickSelectSearchTextBox_UnitsProperty =
        DependencyProperty.Register("ClickSelectSearchTextBox_Units", typeof(string), typeof(ClickSelectSearchTextBox), new UIPropertyMetadata(String.Empty));

      public string ClickSelectSearchTextBox_Units
      {
         get { return (string)GetValue(ClickSelectSearchTextBox_UnitsProperty); }
         set { SetValue(ClickSelectSearchTextBox_UnitsProperty, value); }
      }

      public static readonly DependencyProperty ClickSelectSearchTextBox_SearchCommandProperty =
        DependencyProperty.Register("ClickSelectSearchTextBox_SearchCommand", typeof(ICommand), typeof(ClickSelectSearchTextBox), new UIPropertyMetadata());

      public ICommand ClickSelectSearchTextBox_SearchCommand
      {
         get { return (ICommand)GetValue(ClickSelectSearchTextBox_SearchCommandProperty); }
         set { SetValue(ClickSelectSearchTextBox_SearchCommandProperty, value); }
      }

      public ClickSelectSearchTextBox()
      {
         
      }
   }
}
