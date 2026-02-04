using OtherSideCore.Adapter.Views;
using System;
using System.ComponentModel;
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
          DependencyProperty.Register("MainWindow_ApplicationLogo", typeof(object), typeof(MainWindow), new UIPropertyMetadata(null));

      public object MainWindow_ApplicationLogo
      {
         get { return (object)GetValue(MainWindow_ApplicationLogoProperty); }
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

         DataContextChanged += OnDataContextChanged;
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods

      private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
      {
         if (DataContext is MainWindowViewModel vm)
         {            
            MainWindow_ApplicationName = vm.ApplicationName;
            MainWindow_ApplicationLogo = vm.ApplicationLogoImageSource;
            MainWindow_NavigationMenu.DataContext = vm.NavigationMenuViewModel;

            vm.PropertyChanged += MainWindowViewModel_PropertyChanged;
         }
      }

      private void MainWindowViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
      {
         if (e.PropertyName.Equals(nameof(MainWindowViewModel.CurrentView)))
         {
            var mainWindowViewModel = (MainWindowViewModel)DataContext;

            MainWindow_ViewContent = mainWindowViewModel.CurrentView as UserControl;

            if (MainWindow_ViewContent != null)
            {
               MainWindow_ViewContent.DataContext = mainWindowViewModel.CurrentViewModel;
            }          
         }
      }

      #endregion
   }
}
