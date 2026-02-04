using OtherSideCore.Adapter;
using OtherSideCore.Adapter.DomainObjectInteraction;
using OtherSideCore.Adapter.DomainObjectInteractionViewModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace OtherSideCore.Wpf.CustomControls
{
   public partial class Style : ResourceDictionary
   {
      private const int GRIDVIEW_COLUMN_MIN_WIDTH = 50;

      private void ListView_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
      {
         if (!e.Handled)
         {
            e.Handled = true;
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            eventArg.RoutedEvent = UIElement.MouseWheelEvent;
            eventArg.Source = sender;
            var parent = ((Control)sender).Parent as UIElement;
            parent.RaiseEvent(eventArg);
         }
      }

      private void GridViewColumnHeader_ThumbDragDelta(object sender, DragDeltaEventArgs e)
      {
         Thumb senderAsThumb = e.OriginalSource as Thumb;
         GridViewColumnHeader header = senderAsThumb.TemplatedParent as GridViewColumnHeader;

         if (header.Column != null)
         {
            if (header.Column.ActualWidth + e.HorizontalChange < GRIDVIEW_COLUMN_MIN_WIDTH)
            {
               e.Handled = true;
            }
         }
      }

      private async void DomainObjectSearchListView_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
      {
         var listView = (sender as ListView);
         var selectedDomainObjectSearchResultViewModel = listView.SelectedItem as DomainObjectSearchResultViewModel;

         if (selectedDomainObjectSearchResultViewModel != null)
         {
            var domainObjectBrowserViewModel = listView.DataContext as IDomainObjectBrowserViewModel;

            await domainObjectBrowserViewModel.SelectSearchResultViewModelAsync(selectedDomainObjectSearchResultViewModel);
         }
      }

      private async void DomainObjectSearchListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
      {
         var listView = (sender as ListView);
         var selectedDomainObjectSearchResultViewModel = listView.SelectedItem as DomainObjectSearchResultViewModel;

         if (selectedDomainObjectSearchResultViewModel != null)
         {
            var domainObjectBrowserViewModel = listView.DataContext as IDomainObjectBrowserViewModel;

            if (domainObjectBrowserViewModel.CanShowDomainObjectDetailsEditor(selectedDomainObjectSearchResultViewModel))
            {
               await domainObjectBrowserViewModel.ShowDomainObjectDetailsEditorAsync(selectedDomainObjectSearchResultViewModel);
            }
         }
      }

      private void ComboBox_ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
      {
         var combobox = (sender as ScrollViewer).TemplatedParent as ComboBox;
      }

      private void ComboBoxItem_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
      {
         e.Handled = true;
      }
   }
}
