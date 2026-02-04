using OtherSideCore.Wpf.UserControls.List;
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
   /// Interaction logic for OtherSideCoreProgressBar.xaml
   /// </summary>
   public partial class OtherSideCoreProgressBar : UserControl
   {
      public static readonly DependencyProperty OtherSideCoreProgressBar_MinimumProperty =
          DependencyProperty.Register("OtherSideCoreProgressBar_Minimum", typeof(double), typeof(OtherSideCoreProgressBar), new UIPropertyMetadata(0.0));

      public double OtherSideCoreProgressBar_Minimum
      {
         get { return (double)GetValue(OtherSideCoreProgressBar_MinimumProperty); }
         set { SetValue(OtherSideCoreProgressBar_MinimumProperty, value); }
      }

      public static readonly DependencyProperty OtherSideCoreProgressBar_MaximumProperty =
          DependencyProperty.Register("OtherSideCoreProgressBar_Maximum", typeof(double), typeof(OtherSideCoreProgressBar), new UIPropertyMetadata(2.0, OnValueOrMaximumChanged));

      public double OtherSideCoreProgressBar_Maximum
      {
         get { return (double)GetValue(OtherSideCoreProgressBar_MaximumProperty); }
         set { SetValue(OtherSideCoreProgressBar_MaximumProperty, value); }
      }

      public static readonly DependencyProperty OtherSideCoreProgressBar_ValueProperty =
          DependencyProperty.Register("OtherSideCoreProgressBar_Value", typeof(double), typeof(OtherSideCoreProgressBar), new UIPropertyMetadata(1.0, OnValueOrMaximumChanged));

      public double OtherSideCoreProgressBar_Value
      {
         get { return (double)GetValue(OtherSideCoreProgressBar_ValueProperty); }
         set { SetValue(OtherSideCoreProgressBar_ValueProperty, value); }
      }

      public static readonly DependencyProperty OtherSideCoreProgressBar_IsValueExceededProperty =
         DependencyProperty.Register("OtherSideCoreProgressBar_IsValueExceeded", typeof(bool), typeof(OtherSideCoreProgressBar), new UIPropertyMetadata(false));

      public bool OtherSideCoreProgressBar_IsValueExceeded
      {
         get { return (bool)GetValue(OtherSideCoreProgressBar_IsValueExceededProperty); }
         private set { SetValue(OtherSideCoreProgressBar_IsValueExceededProperty, value); }
      }

      public static readonly DependencyProperty OtherSideCoreProgressBar_BarColorProperty =
          DependencyProperty.Register("OtherSideCoreProgressBar_BarColor", typeof(SolidColorBrush), typeof(OtherSideCoreProgressBar), new UIPropertyMetadata(Brushes.Blue));

      public SolidColorBrush OtherSideCoreProgressBar_BarColor
      {
         get { return (SolidColorBrush)GetValue(OtherSideCoreProgressBar_BarColorProperty); }
         set { SetValue(OtherSideCoreProgressBar_BarColorProperty, value); }
      }

      public OtherSideCoreProgressBar()
      {
         InitializeComponent();
      }

      private static void OnValueOrMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         var progressBar = (OtherSideCoreProgressBar)d;
         progressBar.UpdateIsValueExceeded();
      }

      private void UpdateIsValueExceeded()
      {
         OtherSideCoreProgressBar_IsValueExceeded = OtherSideCoreProgressBar_Value > OtherSideCoreProgressBar_Maximum;
      }
   }
}
