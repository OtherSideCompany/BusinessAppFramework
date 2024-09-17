using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace OtherSideCore.Wpf.CustomControls
{
   public class RepositoryListView : UserControl
   {
      public static readonly DependencyProperty RepositoryListView_AreFiltersVisibleProperty =
                             DependencyProperty.Register("RepositoryListView_AreFiltersVisible", typeof(bool), typeof(RepositoryListView), new UIPropertyMetadata(false));

      public bool RepositoryListView_AreFiltersVisible
      {
         get { return (bool)GetValue(RepositoryListView_AreFiltersVisibleProperty); }
         set { SetValue(RepositoryListView_AreFiltersVisibleProperty, value); }
      }
   }
}
