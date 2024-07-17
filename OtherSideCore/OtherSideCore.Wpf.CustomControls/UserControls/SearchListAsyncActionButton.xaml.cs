using OtherSideCore.Wpf.CustomControls;
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
   /// Interaction logic for SearchListAsyncActionButton.xaml
   /// </summary>
   public partial class SearchListAsyncActionButton : UserControl
   {
      public static readonly DependencyProperty SearchListAsyncActionButton_CommandProperty =
          DependencyProperty.Register("SearchListAsyncActionButton_Command", typeof(ICommand), typeof(SearchListAsyncActionButton), new UIPropertyMetadata(null));

      public ICommand SearchListAsyncActionButton_Command
      {
         get { return (ICommand)GetValue(SearchListAsyncActionButton_CommandProperty); }
         set { SetValue(SearchListAsyncActionButton_CommandProperty, value); }
      }

      public static readonly DependencyProperty SearchListAsyncActionButton_CommandParameterProperty =
          DependencyProperty.Register("SearchListAsyncActionButton_CommandParameter", typeof(object), typeof(SearchListAsyncActionButton), new UIPropertyMetadata(null));

      public object SearchListAsyncActionButton_CommandParameter
      {
         get { return (object)GetValue(SearchListAsyncActionButton_CommandParameterProperty); }
         set { SetValue(SearchListAsyncActionButton_CommandParameterProperty, value); }
      }

      public static readonly DependencyProperty SearchListAsyncActionButton_IconGeometryProperty =
        DependencyProperty.Register("SearchListAsyncActionButton_IconGeometry", typeof(Geometry), typeof(SearchListAsyncActionButton), new UIPropertyMetadata(null));

      public Geometry SearchListAsyncActionButton_IconGeometry
      {
         get { return (Geometry)GetValue(SearchListAsyncActionButton_IconGeometryProperty); }
         set { SetValue(SearchListAsyncActionButton_IconGeometryProperty, value); }
      }
      
      public static readonly DependencyProperty SearchListAsyncActionButton_ImageColorProperty =
        DependencyProperty.Register("SearchListAsyncActionButton_ImageColor", typeof(SolidColorBrush), typeof(SearchListAsyncActionButton), new UIPropertyMetadata(Brushes.Black));

      public SolidColorBrush SearchListAsyncActionButton_ImageColor
      {
         get { return (SolidColorBrush)GetValue(SearchListAsyncActionButton_ImageColorProperty); }
         set { SetValue(SearchListAsyncActionButton_ImageColorProperty, value); }
      }

      public static readonly DependencyProperty SearchListAsyncActionButton_TextProperty =
        DependencyProperty.Register("SearchListAsyncActionButton_Text", typeof(string), typeof(SearchListAsyncActionButton), new UIPropertyMetadata(""));

      public string SearchListAsyncActionButton_Text
      {
         get { return (string)GetValue(SearchListAsyncActionButton_TextProperty); }
         set { SetValue(SearchListAsyncActionButton_TextProperty, value); }
      }

      public static readonly DependencyProperty SearchListAsyncActionButton_ExecutingTaskTextProperty =
        DependencyProperty.Register("SearchListAsyncActionButton_ExecutingTaskText", typeof(string), typeof(SearchListAsyncActionButton), new UIPropertyMetadata(""));

      public string SearchListAsyncActionButton_ExecutingTaskText
      {
         get { return (string)GetValue(SearchListAsyncActionButton_ExecutingTaskTextProperty); }
         set { SetValue(SearchListAsyncActionButton_ExecutingTaskTextProperty, value); }
      }

      public static readonly DependencyProperty SearchListAsyncActionButton_IconSizeProperty =
        DependencyProperty.Register("SearchListAsyncActionButton_IconSize", typeof(int), typeof(SearchListAsyncActionButton), new UIPropertyMetadata(24));

      public int SearchListAsyncActionButton_IconSize
      {
         get { return (int)GetValue(SearchListAsyncActionButton_IconSizeProperty); }
         set { SetValue(SearchListAsyncActionButton_IconSizeProperty, value); }
      }

      public SearchListAsyncActionButton()
      {
         InitializeComponent();
      }
   }
}
