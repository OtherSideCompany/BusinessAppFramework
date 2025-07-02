using OtherSideCore.Adapter.DomainObjectInteractionViewModel;
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

namespace OtherSideCore.Wpf.UserControls.Buttons
{
   /// <summary>
   /// Interaction logic for DomainObjectLink.xaml
   /// </summary>
   public partial class DomainObjectLink : UserControl
   {
      public static readonly DependencyProperty DomainObjectLink_LinkClickedCommandProperty =
         DependencyProperty.Register("DomainObjectLink_LinkClickedCommand", typeof(ICommand), typeof(DomainObjectLink), new UIPropertyMetadata(null));

      public ICommand DomainObjectLink_LinkClickedCommand
      {
         get { return (ICommand)GetValue(DomainObjectLink_LinkClickedCommandProperty); }
         set { SetValue(DomainObjectLink_LinkClickedCommandProperty, value); }
      }

      public static readonly DependencyProperty DomainObjectLink_IdProperty =
         DependencyProperty.Register("DomainObjectLink_Id", typeof(int?), typeof(DomainObjectLink), new UIPropertyMetadata(0));

      public int? DomainObjectLink_Id
      {
         get { return (int?)GetValue(DomainObjectLink_IdProperty); }
         set { SetValue(DomainObjectLink_IdProperty, value); }
      }

      public static readonly DependencyProperty DomainObjectLink_NameProperty =
         DependencyProperty.Register("DomainObjectLink_Name", typeof(string), typeof(DomainObjectLink), new UIPropertyMetadata("-NA-"));

      public string DomainObjectLink_Name
      {
         get { return (string)GetValue(DomainObjectLink_NameProperty); }
         set { SetValue(DomainObjectLink_NameProperty, value); }
      }

      public static readonly DependencyProperty DomainObjectLink_DomainObjectTypeProperty =
    DependencyProperty.Register(nameof(DomainObjectLink_DomainObjectType), typeof(Type), typeof(DomainObjectLink), new PropertyMetadata(null));

      public Type DomainObjectLink_DomainObjectType
      {
         get => (Type)GetValue(DomainObjectLink_DomainObjectTypeProperty);
         set => SetValue(DomainObjectLink_DomainObjectTypeProperty, value);
      }

      public DomainObjectLink()
      {
         InitializeComponent();
      }

      private async void Button_Click(object sender, RoutedEventArgs e)
      {
         var source = sender as DependencyObject;
         var domainObjectInteractionHost = FindDomainObjectInteractionHost(source);

         if (domainObjectInteractionHost != null && DomainObjectLink_Id != null)
         {
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
               await domainObjectInteractionHost.DomainObjectInteractionService.DisplayDomainObjectTreeViewAsync(DomainObjectLink_Id.Value, DomainObjectLink_DomainObjectType, Adapter.DisplayType.SubWindow);
            }
            else if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
            {
               await domainObjectInteractionHost.DomainObjectInteractionService.DisplayDomainObjectDetailsEditorViewAsync(DomainObjectLink_Id.Value, DomainObjectLink_DomainObjectType, Adapter.DisplayType.SubWindow);
            }
            else
            {
               await domainObjectInteractionHost.DomainObjectInteractionService.DisplayDomainObjectAsync(DomainObjectLink_Id.Value, DomainObjectLink_DomainObjectType, Adapter.DisplayType.SubWindow);
            }
         }
      }

      private static IDomainObjectInteractionHost? FindDomainObjectInteractionHost(DependencyObject? start)
      {
         while (start != null)
         {
            if (start is FrameworkElement fe && fe.DataContext is IDomainObjectInteractionHost host)
            {
               return host;
            }

            var parent = VisualTreeHelper.GetParent(start);

            if (parent == null && start is FrameworkElement logicalFe)
            {
               parent = LogicalTreeHelper.GetParent(logicalFe);
            }

            start = parent;
         }

         return null;
      }

      private void LinkActionsPopup_PreviewMouseUp(object sender, MouseButtonEventArgs e)
      {
         LinkActionsPopUp.IsOpen = false;
      }

      private async void OpenAssociatedModule_ButtonClick(object sender, RoutedEventArgs e)
      {
         var source = sender as DependencyObject;
         var domainObjectInteractionHost = FindDomainObjectInteractionHost(source);

         if (domainObjectInteractionHost != null && DomainObjectLink_Id != null)
         {
            await domainObjectInteractionHost.DomainObjectInteractionService.DisplayDomainObjectAsync(DomainObjectLink_Id.Value, DomainObjectLink_DomainObjectType, Adapter.DisplayType.SubWindow);
         }
      }

      private async void OpenTreeView_ButtonClick(object sender, RoutedEventArgs e)
      {
         var source = sender as DependencyObject;
         var domainObjectInteractionHost = FindDomainObjectInteractionHost(source);

         if (domainObjectInteractionHost != null && DomainObjectLink_Id != null)
         {
            await domainObjectInteractionHost.DomainObjectInteractionService.DisplayDomainObjectTreeViewAsync(DomainObjectLink_Id.Value, DomainObjectLink_DomainObjectType, Adapter.DisplayType.SubWindow);
         }
      }

      private async void OpenDetails_ButtonClick(object sender, RoutedEventArgs e)
      {
         var source = sender as DependencyObject;
         var domainObjectInteractionHost = FindDomainObjectInteractionHost(source);

         if (domainObjectInteractionHost != null && DomainObjectLink_Id != null)
         {
            await domainObjectInteractionHost.DomainObjectInteractionService.DisplayDomainObjectDetailsEditorViewAsync(DomainObjectLink_Id.Value, DomainObjectLink_DomainObjectType, Adapter.DisplayType.SubWindow);
         }
      }
   }
}
