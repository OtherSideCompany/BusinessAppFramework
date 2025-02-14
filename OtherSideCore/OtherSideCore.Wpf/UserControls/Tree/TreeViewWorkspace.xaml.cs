using OtherSideCore.Wpf.UserControls.Browser;
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
   /// Interaction logic for TreeViewWorkspace.xaml
   /// </summary>
   public partial class TreeViewWorkspace : UserControl
   {
      public static readonly DependencyProperty TreeViewWorkspace_NameProperty =
          DependencyProperty.Register("TreeViewWorkspace_Name", typeof(string), typeof(TreeViewWorkspace), new UIPropertyMetadata("Unknown"));

      public string TreeViewWorkspace_Name
      {
         get { return (string)GetValue(TreeViewWorkspace_NameProperty); }
         set { SetValue(TreeViewWorkspace_NameProperty, value); }
      }

      public static readonly DependencyProperty TreeViewWorkspace_IconProperty =
          DependencyProperty.Register("TreeViewWorkspace_Icon", typeof(Geometry), typeof(TreeViewWorkspace), new UIPropertyMetadata());

      public string TreeViewWorkspace_Icon
      {
         get { return (string)GetValue(TreeViewWorkspace_IconProperty); }
         set { SetValue(TreeViewWorkspace_IconProperty, value); }
      }

      public static readonly DependencyProperty TreeViewWorkspace_HeaderVisibilityProperty =
          DependencyProperty.Register("TreeViewWorkspace_HeaderVisibility", typeof(Visibility), typeof(TreeViewWorkspace), new UIPropertyMetadata(Visibility.Visible));

      public Visibility TreeViewWorkspace_HeaderVisibility
      {
         get { return (Visibility)GetValue(TreeViewWorkspace_HeaderVisibilityProperty); }
         set { SetValue(TreeViewWorkspace_HeaderVisibilityProperty, value); }
      }

      public static readonly DependencyProperty TreeViewWorkspace_TreeViewProperty =
          DependencyProperty.Register("TreeViewWorkspace_TreeView", typeof(TreeView), typeof(TreeViewWorkspace), new UIPropertyMetadata());

      public TreeView TreeViewWorkspace_TreeView
      {
         get { return (TreeView)GetValue(TreeViewWorkspace_TreeViewProperty); }
         set { SetValue(TreeViewWorkspace_TreeViewProperty, value); }
      }

      public static readonly DependencyProperty TreeViewWorkspace_EditorProperty =
          DependencyProperty.Register("TreeViewWorkspace_Editor", typeof(object), typeof(TreeViewWorkspace), new UIPropertyMetadata());

      public object TreeViewWorkspace_Editor
      {
         get { return (object)GetValue(TreeViewWorkspace_EditorProperty); }
         set { SetValue(TreeViewWorkspace_EditorProperty, value); }
      }

      public static readonly DependencyProperty TreeViewWorkspace_EditorResourcesDictionaryProperty =
          DependencyProperty.Register("TreeViewWorkspace_EditorResourcesDictionary", typeof(ResourceDictionary), typeof(TreeViewWorkspace), new UIPropertyMetadata());

      public ResourceDictionary TreeViewWorkspace_EditorResourcesDictionary
      {
         get { return (ResourceDictionary)GetValue(TreeViewWorkspace_EditorResourcesDictionaryProperty); }
         set { SetValue(TreeViewWorkspace_EditorResourcesDictionaryProperty, value); }
      }

      public static readonly DependencyProperty TreeViewWorkspace_TreeViewMarginProperty =
         DependencyProperty.Register("TreeViewWorkspace_TreeViewMargin", typeof(Thickness), typeof(TreeViewWorkspace), new UIPropertyMetadata(new Thickness(24,16,0,4)));

      public Thickness TreeViewWorkspace_TreeViewMargin
      {
         get { return (Thickness)GetValue(TreeViewWorkspace_TreeViewMarginProperty); }
         set { SetValue(TreeViewWorkspace_TreeViewMarginProperty, value); }
      }

      public TreeViewWorkspace()
      {
         InitializeComponent();
      }
   }
}
