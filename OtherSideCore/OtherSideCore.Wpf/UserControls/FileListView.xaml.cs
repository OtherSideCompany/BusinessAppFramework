using OtherSideCore.Adapter;
using OtherSideCore.Application.Services;
using System;
using System.Collections.Generic;
using System.IO;
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
   /// Interaction logic for FileListView.xaml
   /// </summary>
   public partial class FileListView : UserControl
   {
      public static readonly DependencyProperty FileListView_FileOverColorProperty =
          DependencyProperty.Register("FileListView_FileOverColor", typeof(SolidColorBrush), typeof(FileListView), new UIPropertyMetadata(Brushes.LightBlue));

      public SolidColorBrush FileListView_FileOverColor
      {
         get { return (SolidColorBrush)GetValue(FileListView_FileOverColorProperty); }
         set { SetValue(FileListView_FileOverColorProperty, value); }
      }

      public static readonly DependencyProperty FileListView_DomainObjectFileServiceProperty =
          DependencyProperty.Register("FileListView_DomainObjectFileService", typeof(IDomainObjectFileService), typeof(FileListView), new UIPropertyMetadata());

      public IDomainObjectFileService FileListView_DomainObjectFileService
      {
         get { return (IDomainObjectFileService)GetValue(FileListView_DomainObjectFileServiceProperty); }
         set { SetValue(FileListView_DomainObjectFileServiceProperty, value); }
      }

      public FileListView()
      {
         InitializeComponent();
      }

      private void FileInfoGrid_MouseDown(object sender, MouseButtonEventArgs e)
      {
         if (e.ClickCount == 2 && e.LeftButton == MouseButtonState.Pressed)
         {
            FileListView_DomainObjectFileService?.OpenFile((sender as Grid).DataContext as FileInfo);
         }
      }
   }
}
