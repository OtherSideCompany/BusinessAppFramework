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
      public static readonly DependencyProperty SelectorPropertyEditor_DomainObjectProperty =
        DependencyProperty.Register(nameof(SelectorPropertyEditor_DomainObject), typeof(DomainObject), typeof(SelectorPropertyEditor), new PropertyMetadata(null));

      public DomainObject SelectorPropertyEditor_DomainObject
      {
         get => (DomainObject)GetValue(SelectorPropertyEditor_DomainObjectProperty);
         set => SetValue(SelectorPropertyEditor_DomainObjectProperty, value);
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
