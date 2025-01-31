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

namespace OtherSideCore.Wpf.UserControls.Files
{
   /// <summary>
   /// Interaction logic for ManagedFileListView.xaml
   /// </summary>
   public partial class ManagedFileListView : UserControl
   {
      public static readonly DependencyProperty ManagedFileListView_FileOverColorProperty =
          DependencyProperty.Register("ManagedFileListView_FileOverColor", typeof(SolidColorBrush), typeof(ManagedFileListView), new UIPropertyMetadata(Brushes.LightBlue));

      public SolidColorBrush ManagedFileListView_FileOverColor
      {
         get { return (SolidColorBrush)GetValue(ManagedFileListView_FileOverColorProperty); }
         set { SetValue(ManagedFileListView_FileOverColorProperty, value); }
      }

      public static readonly DependencyProperty ManagedFileListView_DomainObjectFileServiceProperty =
          DependencyProperty.Register("ManagedFileListView_DomainObjectFileService", typeof(IDomainObjectFileService), typeof(ManagedFileListView), new UIPropertyMetadata());

      public IDomainObjectFileService ManagedFileListView_DomainObjectFileService
      {
         get { return (IDomainObjectFileService)GetValue(ManagedFileListView_DomainObjectFileServiceProperty); }
         set { SetValue(ManagedFileListView_DomainObjectFileServiceProperty, value); }
      }

      public ManagedFileListView()
      {
         InitializeComponent();
      }

      private void FileInfoGrid_MouseDown(object sender, MouseButtonEventArgs e)
      {
         if (e.ClickCount == 2 && e.LeftButton == MouseButtonState.Pressed)
         {
            var managedFile = (sender as Grid).DataContext as ManagedFile;
            ManagedFileListView_DomainObjectFileService?.OpenFile(managedFile.PhysicalFile);
         }
      }
   }
}
