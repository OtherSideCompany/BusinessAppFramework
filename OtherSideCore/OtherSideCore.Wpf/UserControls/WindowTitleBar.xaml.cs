using OtherSideCore.Adapter.Views;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace OtherSideCore.Wpf.UserControls
{
   /// <summary>
   /// Interaction logic for WindowTitleBar.xaml
   /// </summary>
   public partial class WindowTitleBar : UserControl
   {
      private Window _parentWindow;
      private bool _restoreIfMove = false;

      public static readonly DependencyProperty WindowTitleBar_ApplicationLogoColorProperty =
          DependencyProperty.Register("WindowTitleBar_ApplicationLogoColor", typeof(SolidColorBrush), typeof(WindowTitleBar), new UIPropertyMetadata(Brushes.Blue));

      public SolidColorBrush WindowTitleBar_ApplicationLogoColor
      {
         get { return (SolidColorBrush)GetValue(WindowTitleBar_ApplicationLogoColorProperty); }
         set { SetValue(WindowTitleBar_ApplicationLogoColorProperty, value); }
      }

      public WindowTitleBar()
      {
         InitializeComponent();

         Loaded += new RoutedEventHandler(WindowTitleBar_Loaded);
      }

      private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
      {
         UpdateWindowSizeControlButtonsVisiblity();
      }

      protected void UpdateWindowSizeControlButtonsVisiblity()
      {
         if (_parentWindow.WindowState == WindowState.Normal)
         {
            MaximizeButton.Visibility = Visibility.Visible;
            RestoreButton.Visibility = Visibility.Collapsed;
         }
         else if (_parentWindow.WindowState == WindowState.Maximized)
         {
            MaximizeButton.Visibility = Visibility.Collapsed;
            RestoreButton.Visibility = Visibility.Visible;
         }
      }

      private void WindowTitleBar_Loaded(object sender, RoutedEventArgs e)
      {
         _parentWindow = Window.GetWindow(this);
         _parentWindow.SizeChanged += Window_SizeChanged;
         UpdateWindowSizeControlButtonsVisiblity();
      }

      private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
      {
         if (e.ClickCount == 2)
         {
            if ((_parentWindow.ResizeMode == ResizeMode.CanResize) || (_parentWindow.ResizeMode == ResizeMode.CanResizeWithGrip))
            {
               SwitchWindowState();
            }

            return;
         }
         else if (_parentWindow.WindowState == WindowState.Maximized)
         {
            _restoreIfMove = true;
            return;
         }

         _parentWindow.DragMove();
      }

      private void Header_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
      {
         _restoreIfMove = false;
      }

      private void Header_MouseMove(object sender, MouseEventArgs e)
      {
         if (_restoreIfMove)
         {
            _restoreIfMove = false;

            var mousePositionInBarCoordinates = e.MouseDevice.GetPosition(this);

            var ratioX = mousePositionInBarCoordinates.X / this.ActualWidth;

            var point = PointToScreen(mousePositionInBarCoordinates);

            _parentWindow.Left = point.X - (_parentWindow.RestoreBounds.Width * ratioX);
            _parentWindow.Top = point.Y - mousePositionInBarCoordinates.Y;

            _parentWindow.WindowState = WindowState.Normal;

            _parentWindow.DragMove();
         }
      }

      private void MinimizeButton_Click(object sender, RoutedEventArgs e)
      {
         _parentWindow.WindowState = WindowState.Minimized;
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
         var windowViewModel = DataContext as WindowViewModel;
         windowViewModel.WindowService.CloseWindow(_parentWindow);
      }

      private void SwitchWindowState()
      {
         if (_parentWindow.WindowState == WindowState.Normal)
         {
            _parentWindow.WindowState = WindowState.Maximized;
         }
         else
         {
            _parentWindow.WindowState = WindowState.Normal;
         }
      }
   }
}
