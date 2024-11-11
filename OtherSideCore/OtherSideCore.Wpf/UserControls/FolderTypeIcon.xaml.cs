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
   /// Interaction logic for FolderTypeIcon.xaml
   /// </summary>
   public partial class FolderTypeIcon : UserControl
   {
      public static readonly DependencyProperty FolderTypeIcon_FolderIconColorProperty =
          DependencyProperty.Register("FolderTypeIcon_FolderIconColor", typeof(SolidColorBrush), typeof(FolderTypeIcon), new UIPropertyMetadata(Brushes.Black));

      public SolidColorBrush FolderTypeIcon_FolderIconColor
      {
         get { return (SolidColorBrush)GetValue(FolderTypeIcon_FolderIconColorProperty); }
         set { SetValue(FolderTypeIcon_FolderIconColorProperty, value); }
      }
      public FolderTypeIcon()
      {
         InitializeComponent();
      }
   }
}
