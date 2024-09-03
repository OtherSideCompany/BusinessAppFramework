using OtherSideCore.Wpf.CustomControls.Extensions;
using System;
using System.ComponentModel;
using System.DirectoryServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OtherSideCore.Wpf.CustomControls
{
   public class SortableGridViewColumn : GridViewColumn
   {
      public static readonly DependencyProperty IsSortedProperty =
        DependencyProperty.RegisterAttached("IsSorted", typeof(bool), typeof(SortableGridViewColumn), new PropertyMetadata(false));

      public bool IsSorted
      {
         get { return (bool)GetValue(IsSortedProperty); }
         set { SetValue(IsSortedProperty, value); }
      }

      public static readonly DependencyProperty SortDirectionProperty =
        DependencyProperty.RegisterAttached("SortDirection", typeof(ListSortDirection), typeof(SortableGridViewColumn), new PropertyMetadata(ListSortDirection.Descending));

      public ListSortDirection SortDirection
      {
         get { return (ListSortDirection)GetValue(SortDirectionProperty); }
         set { SetValue(SortDirectionProperty, value); }
      }

      public static readonly DependencyProperty SortPropertyNameProperty =
        DependencyProperty.RegisterAttached("SortPropertyName", typeof(string), typeof(SortableGridViewColumn), new PropertyMetadata(""));

      public string SortPropertyName
      {
         get { return (string)GetValue(SortPropertyNameProperty); }
         set { SetValue(SortPropertyNameProperty, value); }
      }

      internal void Unsort()
      {
         IsSorted = false;
      }

      internal void Sort(ListSortDirection sortDirection)
      {
         IsSorted = true;
         SortDirection = sortDirection;
      }
   }
}
