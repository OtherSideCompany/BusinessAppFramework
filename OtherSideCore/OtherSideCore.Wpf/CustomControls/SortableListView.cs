using System;
using System.ComponentModel;
using System.DirectoryServices;
using System.Windows;
using System.Windows.Controls;

namespace OtherSideCore.Wpf.CustomControls
{
   public class SortableListView : ListView
   {
      private SortableGridViewColumn _currentSortedColumnHeader;

      public SortableListView() : base()
      {
         AddHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(GridViewColumnHeaderClickedHandler));
      }

      private void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
      {
         var gridViewColumnHeader = e.OriginalSource as GridViewColumnHeader;

         if (gridViewColumnHeader != null)
         {
            if (gridViewColumnHeader.Column is SortableGridViewColumn)
            {
               var direction = ListSortDirection.Ascending;
               var sortableGridViewColumn = gridViewColumnHeader.Column as SortableGridViewColumn;

               if (_currentSortedColumnHeader != null)
               {
                  _currentSortedColumnHeader.Unsort();

                  if (sortableGridViewColumn.Equals(_currentSortedColumnHeader))
                  {
                     direction = sortableGridViewColumn.SortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;
                  }
                  else
                  {
                     direction = ListSortDirection.Ascending;
                  }
               }

               Items.SortDescriptions.Clear();
               Items.SortDescriptions.Add(new SortDescription(sortableGridViewColumn.SortPropertyName, direction));

               _currentSortedColumnHeader = sortableGridViewColumn;
               _currentSortedColumnHeader.Sort(direction);
            }
            else if (gridViewColumnHeader.Column != null)
            {
               throw new Exception("SortableListView requires SortableGridViewColumnHeaders");
            }
         }
      }
   }
}
