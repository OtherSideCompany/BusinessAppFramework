using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OtherSideCore.Wpf.CustomControls
{
   public class SvgViewer : UserControl
   {
      public static readonly DependencyProperty SvgViewer_ImageWidthProperty =
        DependencyProperty.Register("SvgViewer_ImageWidth", typeof(int), typeof(SvgViewer), new UIPropertyMetadata(32));

      public int SvgViewer_ImageWidth
      {
         get { return (int)GetValue(SvgViewer_ImageWidthProperty); }
         set { SetValue(SvgViewer_ImageWidthProperty, value); }
      }

      public static readonly DependencyProperty SvgViewer_ImageHeightProperty =
        DependencyProperty.Register("SvgViewer_ImageHeight", typeof(int), typeof(SvgViewer), new UIPropertyMetadata(32));

      public int SvgViewer_ImageHeight
      {
         get { return (int)GetValue(SvgViewer_ImageHeightProperty); }
         set { SetValue(SvgViewer_ImageHeightProperty, value); }
      }

      public static readonly DependencyProperty SvgViewer_ImageGeometryProperty =
        DependencyProperty.Register("SvgViewer_ImageGeometry", typeof(Geometry), typeof(SvgViewer), new UIPropertyMetadata(null));

      public Geometry SvgViewer_ImageGeometry
      {
         get { return (Geometry)GetValue(SvgViewer_ImageGeometryProperty); }
         set { SetValue(SvgViewer_ImageGeometryProperty, value); }
      }

      public static readonly DependencyProperty SvgViewer_ImageColorProperty =
        DependencyProperty.Register("SvgViewer_ImageColor", typeof(SolidColorBrush), typeof(SvgViewer), new UIPropertyMetadata(Brushes.Black));

      public SolidColorBrush SvgViewer_ImageColor
      {
         get { return (SolidColorBrush)GetValue(SvgViewer_ImageColorProperty); }
         set { SetValue(SvgViewer_ImageColorProperty, value); }
      }

      static SvgViewer()
      {
         DefaultStyleKeyProperty.OverrideMetadata(typeof(SvgViewer), new FrameworkPropertyMetadata(typeof(SvgViewer)));
      }

      public SvgViewer()
      {
         SetResourceReference(StyleProperty, typeof(SvgViewer));
      }
   }
}
