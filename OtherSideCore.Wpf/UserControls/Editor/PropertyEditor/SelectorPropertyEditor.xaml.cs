using OtherSideCore.Application.Search;
using OtherSideCore.Domain.DomainObjects;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SystemAdministration.Desktop.PropertyEditors
{
   /// <summary>
   /// Interaction logic for SelectorPropertyEditor.xaml
   /// </summary>
   public partial class SelectorPropertyEditor : UserControl
   {
      public static readonly DependencyProperty SelectorPropertyEditor_ValueProperty =
        DependencyProperty.Register(nameof(SelectorPropertyEditor_Value), typeof(object), typeof(SelectorPropertyEditor), new PropertyMetadata(null));

      public object SelectorPropertyEditor_Value
      {
         get => GetValue(SelectorPropertyEditor_ValueProperty);
         set => SetValue(SelectorPropertyEditor_ValueProperty, value);
      }

      public static readonly DependencyProperty UserPropertyEditor_DisplaySelectorCommandProperty =
        DependencyProperty.Register(nameof(UserPropertyEditor_DisplaySelectorCommand), typeof(ICommand), typeof(SelectorPropertyEditor), new PropertyMetadata(null));

      public ICommand UserPropertyEditor_DisplaySelectorCommand
      {
         get => (ICommand)GetValue(UserPropertyEditor_DisplaySelectorCommandProperty);
         set => SetValue(UserPropertyEditor_DisplaySelectorCommandProperty, value);
      }

      public SelectorPropertyEditor()
      {
         InitializeComponent();
      }
   }
}
