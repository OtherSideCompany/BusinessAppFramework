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
   /// Interaction logic for FileTypeIcon.xaml
   /// </summary>
   public partial class FileTypeIcon : UserControl
   {
      public static readonly DependencyProperty FileTypeIcon_FileIconColorProperty =
          DependencyProperty.Register("FileTypeIcon_FileIconColor", typeof(SolidColorBrush), typeof(FileTypeIcon), new UIPropertyMetadata(Brushes.Black));

      public SolidColorBrush FileTypeIcon_FileIconColor
      {
         get { return (SolidColorBrush)GetValue(FileTypeIcon_FileIconColorProperty); }
         set { SetValue(FileTypeIcon_FileIconColorProperty, value); }
      }

      public FileTypeIcon()
      {
         InitializeComponent();
      }
   }
}
