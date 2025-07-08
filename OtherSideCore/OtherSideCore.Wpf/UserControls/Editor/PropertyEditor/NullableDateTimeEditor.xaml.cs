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

namespace OtherSideCore.Wpf.UserControls.Editor.PropertyEditor
{
   /// <summary>
   /// Interaction logic for NullableDateTimeEditor.xaml
   /// </summary>
   public partial class NullableDateTimeEditor : UserControl
   {
      public static readonly DependencyProperty NullableDateTimeEditor_SelectedDateProperty =
        DependencyProperty.Register(nameof(NullableDateTimeEditor_SelectedDate), typeof(DateTime?), typeof(NullableDateTimeEditor),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

      public DateTime? NullableDateTimeEditor_SelectedDate
      {
         get => (DateTime?)GetValue(NullableDateTimeEditor_SelectedDateProperty);
         set => SetValue(NullableDateTimeEditor_SelectedDateProperty, value);
      }

      public NullableDateTimeEditor()
      {
         InitializeComponent();
      }
   }
}
