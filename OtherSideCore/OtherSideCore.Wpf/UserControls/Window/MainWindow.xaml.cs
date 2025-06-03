using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OtherSideCore.Wpf.UserControls.Window
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : System.Windows.Window
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

      public static readonly DependencyProperty MainWindow_ApplicationNameProperty =
          DependencyProperty.Register("MainWindow_ApplicationName", typeof(string), typeof(MainWindow), new UIPropertyMetadata("Unnamed App"));

      public string MainWindow_ApplicationName
      {
         get { return (string)GetValue(MainWindow_ApplicationNameProperty); }
         set { SetValue(MainWindow_ApplicationNameProperty, value); }
      }

      public static readonly DependencyProperty MainWindow_ApplicationLogoProperty =
          DependencyProperty.Register("MainWindow_ApplicationLogo", typeof(string), typeof(MainWindow), new UIPropertyMetadata(""));

      public string MainWindow_ApplicationLogo
      {
         get { return (string)GetValue(MainWindow_ApplicationLogoProperty); }
         set { SetValue(MainWindow_ApplicationLogoProperty, value); }
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

      public static readonly DependencyProperty MainWindow_NavigationMenuProperty =
          DependencyProperty.Register("MainWindow_NavigationMenu", typeof(UserControl), typeof(MainWindow), new UIPropertyMetadata(null));

      public UserControl MainWindow_NavigationMenu
      {
         get { return (UserControl)GetValue(MainWindow_NavigationMenuProperty); }
         set { SetValue(MainWindow_NavigationMenuProperty, value); }
      }

      #endregion

      #region Constructor

      public MainWindow()
      {
         InitializeComponent();

         MainWindow_NavigationMenu = new NavigationMenu();
      }

      #endregion

      #region Public Methods


      
      #endregion
   }
}
