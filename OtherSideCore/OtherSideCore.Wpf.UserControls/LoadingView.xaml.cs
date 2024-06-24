using System;
using System.Collections.Generic;
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

      public LoadingView()
      {
         InitializeComponent();
      }
   }
}
