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

namespace OtherSideCore.Wpf.UserControls.Workflows
{
   /// <summary>
   /// Interaction logic for ProcessWorkflowHeader.xaml
   /// </summary>
   public partial class ProcessWorkflowHeader : UserControl
   {
      public static readonly DependencyProperty ProcessWorkflowHeader_CompletedColorProperty =
          DependencyProperty.Register("ProcessWorkflowHeader_CompletedColor", typeof(SolidColorBrush), typeof(ProcessWorkflowHeader), new UIPropertyMetadata(Brushes.Green));

      public SolidColorBrush ProcessWorkflowHeader_CompletedColor
      {
         get { return (SolidColorBrush)GetValue(ProcessWorkflowHeader_CompletedColorProperty); }
         set { SetValue(ProcessWorkflowHeader_CompletedColorProperty, value); }
      }

      public static readonly DependencyProperty ProcessWorkflowHeader_SelectedColorProperty =
          DependencyProperty.Register("ProcessWorkflowHeader_SelectedColor", typeof(SolidColorBrush), typeof(ProcessWorkflowHeader), new UIPropertyMetadata(Brushes.Blue));

      public SolidColorBrush ProcessWorkflowHeader_SelectedColor
      {
         get { return (SolidColorBrush)GetValue(ProcessWorkflowHeader_SelectedColorProperty); }
         set { SetValue(ProcessWorkflowHeader_SelectedColorProperty, value); }
      }

      public ProcessWorkflowHeader()
      {
         InitializeComponent();
      }
   }
}
