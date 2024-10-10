using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace OtherSideCore.Wpf.UserControls
{
   /// <summary>
   /// Interaction logic for MainWindowTitleBar.xaml
   /// </summary>
   public partial class MainWindowTitleBar : UserControl
   {
      private bool m_RestoreIfMove = false;

      public static readonly DependencyProperty MainWindowTitleBar_ApplicationLogoColorProperty =
          DependencyProperty.Register("MainWindowTitleBar_ApplicationLogoColor", typeof(SolidColorBrush), typeof(MainWindowTitleBar), new UIPropertyMetadata(Brushes.Blue));

      public SolidColorBrush MainWindowTitleBar_ApplicationLogoColor
      {
         get { return (SolidColorBrush)GetValue(MainWindowTitleBar_ApplicationLogoColorProperty); }
         set { SetValue(MainWindowTitleBar_ApplicationLogoColorProperty, value); }
      }

      public MainWindowTitleBar()
      {
         InitializeComponent();

         UpdateWindowSizeControlButtonsVisiblity();
      }

      private void MainWindowTitleBar_Loaded(object sender, RoutedEventArgs e)
      {
         System.Windows.Application.Current.MainWindow.SizeChanged += MainWindow_SizeChanged;
      }

      private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
      {
         UpdateWindowSizeControlButtonsVisiblity();
      }

      private void UpdateWindowSizeControlButtonsVisiblity()
      {
         if (System.Windows.Application.Current.MainWindow.WindowState == WindowState.Normal)
         {
            MaximizeButton.Visibility = Visibility.Visible;
            RestoreButton.Visibility = Visibility.Collapsed;
         }
         else if (System.Windows.Application.Current.MainWindow.WindowState == WindowState.Maximized)
         {
            MaximizeButton.Visibility = Visibility.Collapsed;
            RestoreButton.Visibility = Visibility.Visible;
         }
      }

      private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
      {
         if (e.ClickCount == 2)
         {
            if ((System.Windows.Application.Current.MainWindow.ResizeMode == ResizeMode.CanResize) || (System.Windows.Application.Current.MainWindow.ResizeMode == ResizeMode.CanResizeWithGrip))
            {
               SwitchWindowState();
            }

            return;
         }
         else if (System.Windows.Application.Current.MainWindow.WindowState == WindowState.Maximized)
         {
            m_RestoreIfMove = true;
            return;
         }

         System.Windows.Application.Current.MainWindow.DragMove();
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

            System.Windows.Application.Current.MainWindow.Left = point.X - (System.Windows.Application.Current.MainWindow.RestoreBounds.Width * ratioX);
            System.Windows.Application.Current.MainWindow.Top = point.Y - mousePositionInBarCoordinates.Y;

            System.Windows.Application.Current.MainWindow.WindowState = WindowState.Normal;

            System.Windows.Application.Current.MainWindow.DragMove();
         }
      }

      private void MinimizeButton_Click(object sender, RoutedEventArgs e)
      {
         System.Windows.Application.Current.MainWindow.WindowState = WindowState.Minimized;
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
         System.Windows.Application.Current.Shutdown();
      }

      private void SwitchWindowState()
      {
         if (System.Windows.Application.Current.MainWindow.WindowState == WindowState.Normal)
         {
            System.Windows.Application.Current.MainWindow.WindowState = WindowState.Maximized;
         }
         else
         {
            System.Windows.Application.Current.MainWindow.WindowState = WindowState.Normal;
         }
      }
   }
}
