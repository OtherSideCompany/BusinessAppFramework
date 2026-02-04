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
   /// Interaction logic for NullableIntegerEditor.xaml
   /// </summary>
   public partial class NullableIntegerEditor : UserControl, ICustomSetCommand
   {
      public static readonly DependencyProperty NullableIntegerEditor_ValueProperty =
        DependencyProperty.Register(nameof(NullableIntegerEditor_Value), typeof(int?), typeof(NullableIntegerEditor),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

      public int? NullableIntegerEditor_Value
      {
         get => (int?)GetValue(NullableIntegerEditor_ValueProperty);
         set => SetValue(NullableIntegerEditor_ValueProperty, value);
      }

      public static readonly DependencyProperty CustomSetCommandProperty =
         DependencyProperty.Register(nameof(CustomSetCommand), typeof(ICommand), typeof(NullableIntegerEditor), new PropertyMetadata(null));

      public ICommand CustomSetCommand
      {
         get => (ICommand)GetValue(CustomSetCommandProperty);
         set => SetValue(CustomSetCommandProperty, value);
      }

      public static readonly DependencyProperty CustomSetCommandIconResourceProperty =
         DependencyProperty.Register(nameof(CustomSetCommandIconResource), typeof(string), typeof(NullableIntegerEditor), new PropertyMetadata(null));

      public string CustomSetCommandIconResource
      {
         get => (string)GetValue(CustomSetCommandIconResourceProperty);
         set => SetValue(CustomSetCommandIconResourceProperty, value);
      }

      public NullableIntegerEditor()
      {
         InitializeComponent();
      }
   }
}
