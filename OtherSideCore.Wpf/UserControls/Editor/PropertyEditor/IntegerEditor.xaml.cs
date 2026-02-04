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
   /// Interaction logic for IntegerEditor.xaml
   /// </summary>
   public partial class IntegerEditor : UserControl, ICustomSetCommand
   {
      public static readonly DependencyProperty IntegerEditor_ValueProperty =
        DependencyProperty.Register(nameof(IntegerEditor_Value), typeof(int?), typeof(IntegerEditor),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

      public int? IntegerEditor_Value
      {
         get => (int?)GetValue(IntegerEditor_ValueProperty);
         set => SetValue(IntegerEditor_ValueProperty, value);
      }

      public static readonly DependencyProperty CustomSetCommandProperty =
         DependencyProperty.Register(nameof(CustomSetCommand), typeof(ICommand), typeof(IntegerEditor), new PropertyMetadata(null));

      public ICommand CustomSetCommand
      {
         get => (ICommand)GetValue(CustomSetCommandProperty);
         set => SetValue(CustomSetCommandProperty, value);
      }

      public static readonly DependencyProperty CustomSetCommandIconResourceProperty =
         DependencyProperty.Register(nameof(CustomSetCommandIconResource), typeof(string), typeof(IntegerEditor), new PropertyMetadata(null));

      public string CustomSetCommandIconResource
      {
         get => (string)GetValue(CustomSetCommandIconResourceProperty);
         set => SetValue(CustomSetCommandIconResourceProperty, value);
      }

      public IntegerEditor()
      {
         InitializeComponent();
      }
   }
}
