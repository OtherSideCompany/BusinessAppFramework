using System.Windows;
using System.Windows.Controls;

using System.Windows.Media;

namespace OtherSideCore.Wpf.UserControls
{
   /// <summary>
   /// Interaction logic for SubWindow.xaml
   /// </summary>
   public partial class SubWindow : Window
   {
      public static readonly DependencyProperty SubWindow_ApplicationLogoColorProperty =
          DependencyProperty.Register("SubWindow_ApplicationLogoColor", typeof(SolidColorBrush), typeof(SubWindow), new UIPropertyMetadata(Brushes.Blue));

      public SolidColorBrush SubWindow_ApplicationLogoColor
      {
         get { return (SolidColorBrush)GetValue(SubWindow_ApplicationLogoColorProperty); }
         set { SetValue(SubWindow_ApplicationLogoColorProperty, value); }
      }

      public static readonly DependencyProperty SubWindow_ViewContentProperty =
          DependencyProperty.Register("SubWindow_ViewContent", typeof(UserControl), typeof(SubWindow), new UIPropertyMetadata(null));

      public UserControl SubWindow_ViewContent
      {
         get { return (UserControl)GetValue(SubWindow_ViewContentProperty); }
         set { SetValue(SubWindow_ViewContentProperty, value); }
      }

      public static readonly DependencyProperty SubWindow_ModalContentProperty =
          DependencyProperty.Register("SubWindow_ModalContent", typeof(UserControl), typeof(SubWindow), new UIPropertyMetadata(null));

      public UserControl SubWindow_ModalContent
      {
         get { return (UserControl)GetValue(SubWindow_ModalContentProperty); }
         set { SetValue(SubWindow_ModalContentProperty, value); }
      }

      public SubWindow()
      {
         InitializeComponent();
      }
   }
}
