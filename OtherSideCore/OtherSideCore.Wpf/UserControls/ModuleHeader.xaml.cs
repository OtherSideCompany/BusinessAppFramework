using OtherSideCore.Wpf.CustomControls;
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
   /// Interaction logic for ModuleHeader.xaml
   /// </summary>
   public partial class ModuleHeader : UserControl
   {
      public static readonly DependencyProperty ModuleHeader_NameProperty =
          DependencyProperty.Register("ModuleHeader_Name", typeof(string), typeof(ModuleHeader), new UIPropertyMetadata("Nouveau module"));

      public string ModuleHeader_Name
      {
         get { return (string)GetValue(ModuleHeader_NameProperty); }
         set { SetValue(ModuleHeader_NameProperty, value); }
      }

      public static readonly DependencyProperty ModuleHeader_IconGeometryProperty =
        DependencyProperty.Register("ModuleHeader_IconGeometry", typeof(Geometry), typeof(ModuleHeader), new UIPropertyMetadata(null));

      public Geometry ModuleHeader_IconGeometry
      {
         get { return (Geometry)GetValue(ModuleHeader_IconGeometryProperty); }
         set { SetValue(ModuleHeader_IconGeometryProperty, value); }
      }

      public static readonly DependencyProperty ModuleHeader_ForegroundProperty =
        DependencyProperty.Register("ModuleHeader_Foreground", typeof(SolidColorBrush), typeof(ModuleHeader), new UIPropertyMetadata(Brushes.Blue));

      public SolidColorBrush ModuleHeader_Foreground
      {
         get { return (SolidColorBrush)GetValue(ModuleHeader_ForegroundProperty); }
         set { SetValue(ModuleHeader_ForegroundProperty, value); }
      }



      public ModuleHeader()
      {
         InitializeComponent();
      }
   }
}
