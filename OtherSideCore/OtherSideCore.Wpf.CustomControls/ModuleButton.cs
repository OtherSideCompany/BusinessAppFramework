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
   public class ModuleButton : Button
   {
      public static readonly DependencyProperty ModuleButton_HoverColorProperty =
        DependencyProperty.Register("ModuleButton_HoverColor", typeof(Brush), typeof(ModuleButton), new UIPropertyMetadata(Brushes.Transparent));

      public Brush ModuleButton_HoverColor
      {
         get { return (Brush)GetValue(ModuleButton_HoverColorProperty); }
         set { SetValue(ModuleButton_HoverColorProperty, value); }
      }

      public static readonly DependencyProperty ModuleButton_PressedColorProperty =
        DependencyProperty.Register("ModuleButton_PressedColor", typeof(Brush), typeof(ModuleButton), new UIPropertyMetadata(Brushes.Transparent));

      public Brush ModuleButton_PressedColor
      {
         get { return (Brush)GetValue(ModuleButton_PressedColorProperty); }
         set { SetValue(ModuleButton_PressedColorProperty, value); }
      }

   }
}
