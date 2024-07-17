using OtherSideCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

      private void SideMenuListView_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
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

      private void ModelObjectSearchListView_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
      {
         var listView = (sender as ListView);
         var selectedModelObjectViewModel = listView.SelectedItem as ModelObjectViewModel;

         if (selectedModelObjectViewModel != null)
         {
            var repositoryManagerViewModel = listView.DataContext as IRepositoryManagerViewModel;

            if (repositoryManagerViewModel != null && repositoryManagerViewModel.SelectSearchResultCommandAsync.CanExecute(selectedModelObjectViewModel))
            {
               repositoryManagerViewModel.SelectSearchResultCommandAsync.Execute(selectedModelObjectViewModel);
            }
         }
      }
   }
}
