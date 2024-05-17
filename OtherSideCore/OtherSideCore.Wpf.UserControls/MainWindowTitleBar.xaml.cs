using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
   /// Interaction logic for MainWindowTitleBar.xaml
   /// </summary>
   public partial class MainWindowTitleBar : UserControl
   {
      private bool m_RestoreIfMove = false;

      public MainWindowTitleBar()
      {
         InitializeComponent();

         UpdateWindowSizeControlButtonsVisiblity();
      }

      private void MainWindowTitleBar_Loaded(object sender, RoutedEventArgs e)
      {
         Application.Current.MainWindow.SizeChanged += MainWindow_SizeChanged;
      }

      private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
      {
         UpdateWindowSizeControlButtonsVisiblity();
      }

      private void UpdateWindowSizeControlButtonsVisiblity()
      {
         if (Application.Current.MainWindow.WindowState == WindowState.Normal)
         {
            MaximizeButton.Visibility = Visibility.Visible;
            RestoreButton.Visibility = Visibility.Collapsed;
         }
         else if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
         {
            MaximizeButton.Visibility = Visibility.Collapsed;
            RestoreButton.Visibility = Visibility.Visible;
         }
      }

      private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
      {
         if (e.ClickCount == 2)
         {
            if ((Application.Current.MainWindow.ResizeMode == ResizeMode.CanResize) || (Application.Current.MainWindow.ResizeMode == ResizeMode.CanResizeWithGrip))
            {
               SwitchWindowState();
            }

            return;
         }
         else if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
         {
            m_RestoreIfMove = true;
            return;
         }

         Application.Current.MainWindow.DragMove();
      }

      private void Header_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
      {
         m_RestoreIfMove = false;
      }

      private void Header_MouseMove(object sender, MouseEventArgs e)
      {
         if (m_RestoreIfMove)
         {
            m_RestoreIfMove = false;

            var mousePositionInBarCoordinates = e.MouseDevice.GetPosition(this);

            var ratioX = mousePositionInBarCoordinates.X / this.ActualWidth;

            var point = PointToScreen(mousePositionInBarCoordinates);

            Application.Current.MainWindow.Left = point.X - (Application.Current.MainWindow.RestoreBounds.Width * ratioX);
            Application.Current.MainWindow.Top = point.Y - mousePositionInBarCoordinates.Y;

            Application.Current.MainWindow.WindowState = WindowState.Normal;

            Application.Current.MainWindow.DragMove();
         }
      }

      private void MinimizeButton_Click(object sender, RoutedEventArgs e)
      {
         Application.Current.MainWindow.WindowState = WindowState.Minimized;
      }

      private void MaximizeButton_Click(object sender, RoutedEventArgs e)
      {
         SwitchWindowState();
      }

      private void RestoreButton_Click(object sender, RoutedEventArgs e)
      {
         SwitchWindowState();
      }

      private void CloseButton_Click(object sender, RoutedEventArgs e)
      {
         Application.Current.Shutdown();
      }

      private void SwitchWindowState()
      {
         if (Application.Current.MainWindow.WindowState == WindowState.Normal)
         {
            Application.Current.MainWindow.WindowState = WindowState.Maximized;
         }
         else
         {
            Application.Current.MainWindow.WindowState = WindowState.Normal;
         }
      }
   }
}
