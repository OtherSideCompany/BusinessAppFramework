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
   /// Interaction logic for FolderListView.xaml
   /// </summary>
   public partial class FolderListView : UserControl
   {
      public static readonly DependencyProperty FolderListView_FolderOverColorProperty =
          DependencyProperty.Register("FolderListView_FolderOverColor", typeof(SolidColorBrush), typeof(FolderListView), new UIPropertyMetadata(Brushes.LightBlue));

      public SolidColorBrush FolderListView_FolderOverColor
      {
         get { return (SolidColorBrush)GetValue(FolderListView_FolderOverColorProperty); }
         set { SetValue(FolderListView_FolderOverColorProperty, value); }
      }

      public static readonly DependencyProperty FolderListView_DomainObjectFileServiceProperty =
          DependencyProperty.Register("FolderListView_DomainObjectFileService", typeof(IDomainObjectFileService), typeof(FolderListView), new UIPropertyMetadata());

      public IDomainObjectFileService FolderListView_DomainObjectFileService
      {
         get { return (IDomainObjectFileService)GetValue(FolderListView_DomainObjectFileServiceProperty); }
         set { SetValue(FolderListView_DomainObjectFileServiceProperty, value); }
      }

      public FolderListView()
      {
         InitializeComponent();
      }

      private void DirectoryInfoGrid_MouseDown(object sender, MouseButtonEventArgs e)
      {
         if (e.ClickCount == 2 && e.LeftButton == MouseButtonState.Pressed)
         {
            FolderListView_DomainObjectFileService.OpenFolder((sender as Grid).DataContext as DirectoryInfo);
         }
      }
   }
}
