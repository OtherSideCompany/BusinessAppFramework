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

namespace OtherSideCore.Wpf.UserControls.Editor.PropertyEditor
{
   /// <summary>
   /// Interaction logic for BooleanEditor.xaml
   /// </summary>
   public partial class BooleanEditor : UserControl, ICustomSetCommand
   {
      public static readonly DependencyProperty BooleanEditor_IsCheckedProperty =
        DependencyProperty.Register(nameof(BooleanEditor_IsChecked), typeof(bool), typeof(BooleanEditor),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

      public bool BooleanEditor_IsChecked
      {
         get => (bool)GetValue(BooleanEditor_IsCheckedProperty);
         set => SetValue(BooleanEditor_IsCheckedProperty, value);
      }

      public static readonly DependencyProperty CustomSetCommandProperty =
         DependencyProperty.Register(nameof(CustomSetCommand), typeof(ICommand), typeof(BooleanEditor), new PropertyMetadata(null));

      public ICommand CustomSetCommand
      {
         get => (ICommand)GetValue(CustomSetCommandProperty);
         set => SetValue(CustomSetCommandProperty, value);
      }

      public static readonly DependencyProperty CustomSetCommandIconResourceProperty =
         DependencyProperty.Register(nameof(CustomSetCommandIconResource), typeof(string), typeof(BooleanEditor), new PropertyMetadata(null));

      public string CustomSetCommandIconResource
      {
         get => (string)GetValue(CustomSetCommandIconResourceProperty);
         set => SetValue(CustomSetCommandIconResourceProperty, value);
      }
      
      public BooleanEditor()
      {
         InitializeComponent();
      }
   }
}
