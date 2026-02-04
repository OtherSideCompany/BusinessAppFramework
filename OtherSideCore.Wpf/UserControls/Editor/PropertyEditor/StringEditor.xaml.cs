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
   /// Interaction logic for StringEditor.xaml
   /// </summary>
   public partial class StringEditor : UserControl, ICustomSetCommand
   {
      public static readonly DependencyProperty StringEditor_TextProperty =
        DependencyProperty.Register(nameof(StringEditor_Text), typeof(string), typeof(StringEditor),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

      public string StringEditor_Text
      {
         get => (string)GetValue(StringEditor_TextProperty);
         set => SetValue(StringEditor_TextProperty, value);
      }

      public static readonly DependencyProperty CustomSetCommandProperty =
         DependencyProperty.Register(nameof(CustomSetCommand), typeof(ICommand), typeof(StringEditor), new PropertyMetadata(null));

      public ICommand CustomSetCommand
      {
         get => (ICommand)GetValue(CustomSetCommandProperty);
         set => SetValue(CustomSetCommandProperty, value);
      }

      public static readonly DependencyProperty CustomSetCommandIconResourceProperty =
         DependencyProperty.Register(nameof(CustomSetCommandIconResource), typeof(string), typeof(StringEditor), new PropertyMetadata(null));

      public string CustomSetCommandIconResource
      {
         get => (string)GetValue(CustomSetCommandIconResourceProperty);
         set => SetValue(CustomSetCommandIconResourceProperty, value);
      }

      public StringEditor()
      {
         InitializeComponent();
      }
   }
}
