using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace OtherSideCore.Wpf.UserControls
{
   /// <summary>
   /// Interaction logic for LoadingView.xaml
   /// </summary>
   public partial class LoadingView : UserControl
   {
      public static readonly DependencyProperty LoadingView_IconColorProperty =
          DependencyProperty.Register("LoadingView_IconColor", typeof(SolidColorBrush), typeof(LoadingView), new UIPropertyMetadata(Brushes.Black));

      public SolidColorBrush LoadingView_IconColor
      {
         get { return (SolidColorBrush)GetValue(LoadingView_IconColorProperty); }
         set { SetValue(LoadingView_IconColorProperty, value); }
      }

      public static readonly DependencyProperty LoadingView_IconSizeProperty =
          DependencyProperty.Register("LoadingView_IconSize", typeof(int), typeof(LoadingView), new UIPropertyMetadata(32));

      public int LoadingView_IconSize
      {
         get { return (int)GetValue(LoadingView_IconSizeProperty); }
         set { SetValue(LoadingView_IconSizeProperty, value); }
      }

      public static readonly DependencyProperty LoadingView_DisplayCaptionProperty =
          DependencyProperty.Register("LoadingView_DisplayCaption", typeof(bool), typeof(LoadingView), new UIPropertyMetadata(true));

      public bool LoadingView_DisplayCaption
      {
         get { return (bool)GetValue(LoadingView_DisplayCaptionProperty); }
         set { SetValue(LoadingView_DisplayCaptionProperty, value); }
      }

      public static readonly DependencyProperty LoadingView_CancelCommandProperty =
          DependencyProperty.Register("LoadingView_CancelCommand", typeof(ICommand), typeof(LoadingView), new UIPropertyMetadata(null));

      public ICommand LoadingView_CancelCommand
      {
         get { return (ICommand)GetValue(LoadingView_CancelCommandProperty); }
         set { SetValue(LoadingView_CancelCommandProperty, value); }
      }

      public LoadingView()
      {
         InitializeComponent();
      }
   }
}
