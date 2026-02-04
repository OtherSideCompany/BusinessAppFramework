using OtherSideCore.Adapter.DomainObjectInteraction;
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

namespace OtherSideCore.Wpf.UserControls.Tree
{
   /// <summary>
   /// Interaction logic for TreeViewNode.xaml
   /// </summary>
   public partial class TreeViewNode : UserControl
   {
      public static readonly DependencyProperty TreeViewNode_ContentProperty =
          DependencyProperty.Register("TreeViewNode_Content", typeof(object), typeof(TreeViewNode), new UIPropertyMetadata());

      public object TreeViewNode_Content
      {
         get { return (object)GetValue(TreeViewNode_ContentProperty); }
         set { SetValue(TreeViewNode_ContentProperty, value); }
      }

      public static readonly DependencyProperty TreeViewNode_IsSelectableProperty =
          DependencyProperty.Register("TreeViewNode_IsSelectable", typeof(bool), typeof(TreeViewNode), new UIPropertyMetadata(true));

      public bool TreeViewNode_IsSelectable
      {
         get { return (bool)GetValue(TreeViewNode_IsSelectableProperty); }
         set { SetValue(TreeViewNode_IsSelectableProperty, value); }
      }

      public TreeViewNode()
      {
         InitializeComponent();
      }

      private void Grid_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
      {
         if (TreeViewNode_IsSelectable)
         {
            if (!(e.OriginalSource is Button) && ((Grid)sender).DataContext is IDomainObjectTreeNodeViewModel domainObjectTreeViewNode)
            {
               domainObjectTreeViewNode.RequestSelection();
            }
         }
      }
   }
}
