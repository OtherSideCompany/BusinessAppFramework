using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OtherSideCore.Wpf.UserControls
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
      #region Fields

      

      #endregion

      #region Properties

      public static readonly DependencyProperty MainWindow_ApplicationLogoColorProperty =
          DependencyProperty.Register("MainWindow_ApplicationLogoColor", typeof(SolidColorBrush), typeof(MainWindow), new UIPropertyMetadata(Brushes.Blue));

      public SolidColorBrush MainWindow_ApplicationLogoColor
      {
         get { return (SolidColorBrush)GetValue(MainWindow_ApplicationLogoColorProperty); }
         set { SetValue(MainWindow_ApplicationLogoColorProperty, value); }
      }

      public static readonly DependencyProperty MainWindow_ViewContentProperty =
          DependencyProperty.Register("MainWindow_ViewContent", typeof(UserControl), typeof(MainWindow), new UIPropertyMetadata(null));

      public UserControl MainWindow_ViewContent
      {
         get { return (UserControl)GetValue(MainWindow_ViewContentProperty); }
         set { SetValue(MainWindow_ViewContentProperty, value); }
      }

      public static readonly DependencyProperty MainWindow_ModalContentProperty =
          DependencyProperty.Register("MainWindow_ModalContent", typeof(UserControl), typeof(MainWindow), new UIPropertyMetadata(null));

      public UserControl MainWindow_ModalContent
      {
         get { return (UserControl)GetValue(MainWindow_ModalContentProperty); }
         set { SetValue(MainWindow_ModalContentProperty, value); }
      }

      #endregion

      #region Constructor

      public MainWindow()
      {
         InitializeComponent();
      }

      #endregion

      #region Public Methods


      
      #endregion
   }
}
