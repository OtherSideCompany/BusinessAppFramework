using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OtherSideCore.Wpf.CustomControls
{
   public class InfoPanelCategory : System.Windows.Controls.UserControl
   {
      public static readonly DependencyProperty InfoPanelCategory_IsExpandedProperty =
        DependencyProperty.Register("InfoPanelCategory_IsExpanded", typeof(bool), typeof(InfoPanelCategory), new UIPropertyMetadata(false));

      public bool InfoPanelCategory_IsExpanded
      {
         get { return (bool)GetValue(InfoPanelCategory_IsExpandedProperty); }
         set { SetValue(InfoPanelCategory_IsExpandedProperty, value); }
      }

      public static readonly DependencyProperty InfoPanelCategory_HeaderNameProperty =
        DependencyProperty.Register("InfoPanelCategory_HeaderName", typeof(string), typeof(InfoPanelCategory), new UIPropertyMetadata("Nouveau panneau"));

      public string InfoPanelCategory_HeaderName
      {
         get { return (string)GetValue(InfoPanelCategory_HeaderNameProperty); }
         set { SetValue(InfoPanelCategory_HeaderNameProperty, value); }
      }

      public static readonly DependencyProperty InfoPanelCategory_DisplaySeparatorProperty =
        DependencyProperty.Register("InfoPanelCategory_DisplaySeparator", typeof(bool), typeof(InfoPanelCategory), new UIPropertyMetadata(true));

      public bool InfoPanelCategory_DisplaySeparator
      {
         get { return (bool)GetValue(InfoPanelCategory_DisplaySeparatorProperty); }
         set { SetValue(InfoPanelCategory_DisplaySeparatorProperty, value); }
      }

      static InfoPanelCategory()
      {
         DefaultStyleKeyProperty.OverrideMetadata(typeof(InfoPanelCategory), new FrameworkPropertyMetadata(typeof(InfoPanelCategory)));
      }
   }
}
