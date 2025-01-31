using OtherSideCore.Adapter.DomainObjectInteraction;
using OtherSideCore.Adapter.Views;
using OtherSideCore.Wpf.UserControls.Filters;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace OtherSideCore.Wpf.UserControls.Browser
{
   /// <summary>
   /// Interaction logic for SingleDomainObjectBrowser.xaml
   /// </summary>
   public partial class SingleDomainObjectBrowser : UserControl
   {
      public static readonly DependencyProperty SingleDomainObjectBrowser_ConstraintsContentProperty =
          DependencyProperty.Register("SingleDomainObjectBrowser_ConstraintsContent", typeof(object), typeof(SingleDomainObjectBrowser), new UIPropertyMetadata());

      public object SingleDomainObjectBrowser_ConstraintsContent
      {
         get { return GetValue(SingleDomainObjectBrowser_ConstraintsContentProperty); }
         set { SetValue(SingleDomainObjectBrowser_ConstraintsContentProperty, value); }
      }

      public static readonly DependencyProperty SingleDomainObjectBrowser_MoreActionsContentProperty =
          DependencyProperty.Register("SingleDomainObjectBrowser_MoreActionsContent", typeof(object), typeof(SingleDomainObjectBrowser), new UIPropertyMetadata());

      public object SingleDomainObjectBrowser_MoreActionsContent
      {
         get { return GetValue(SingleDomainObjectBrowser_MoreActionsContentProperty); }
         set { SetValue(SingleDomainObjectBrowser_MoreActionsContentProperty, value); }
      }

      public static readonly DependencyProperty SingleDomainObjectBrowser_FiltersContentProperty =
          DependencyProperty.Register("SingleDomainObjectBrowser_FiltersContent", typeof(object), typeof(SingleDomainObjectBrowser), new UIPropertyMetadata());

      public object SingleDomainObjectBrowser_FiltersContent
      {
         get { return GetValue(SingleDomainObjectBrowser_FiltersContentProperty); }
         set { SetValue(SingleDomainObjectBrowser_FiltersContentProperty, value); }
      }

      public static readonly DependencyProperty SingleDomainObjectBrowser_ListContentProperty =
          DependencyProperty.Register("SingleDomainObjectBrowser_ListContent", typeof(object), typeof(SingleDomainObjectBrowser), new UIPropertyMetadata());

      public object SingleDomainObjectBrowser_ListContent
      {
         get { return GetValue(SingleDomainObjectBrowser_ListContentProperty); }
         set { SetValue(SingleDomainObjectBrowser_ListContentProperty, value); }
      }

      public static readonly DependencyProperty SingleDomainObjectBrowser_EditorContentProperty =
          DependencyProperty.Register("SingleDomainObjectBrowser_EditorContent", typeof(object), typeof(SingleDomainObjectBrowser), new UIPropertyMetadata());

      public object SingleDomainObjectBrowser_EditorContent
      {
         get { return GetValue(SingleDomainObjectBrowser_EditorContentProperty); }
         set { SetValue(SingleDomainObjectBrowser_EditorContentProperty, value); }
      }

      public static readonly DependencyProperty SingleDomainObjectBrowser_NameProperty =
          DependencyProperty.Register("SingleDomainObjectBrowser_Name", typeof(string), typeof(SingleDomainObjectBrowser), new UIPropertyMetadata("Unknown"));

      public string SingleDomainObjectBrowser_Name
      {
         get { return (string)GetValue(SingleDomainObjectBrowser_NameProperty); }
         set { SetValue(SingleDomainObjectBrowser_NameProperty, value); }
      }

      public static readonly DependencyProperty SingleDomainObjectBrowser_IconGeometryProperty =
          DependencyProperty.Register("SingleDomainObjectBrowser_IconGeometry", typeof(Geometry), typeof(SingleDomainObjectBrowser), new UIPropertyMetadata());

      public Geometry SingleDomainObjectBrowser_IconGeometry
      {
         get { return (Geometry)GetValue(SingleDomainObjectBrowser_IconGeometryProperty); }
         set { SetValue(SingleDomainObjectBrowser_IconGeometryProperty, value); }
      }

      public static readonly DependencyProperty SingleDomainObjectBrowser_CreateCommandProperty =
          DependencyProperty.Register("SingleDomainObjectBrowser_CreateCommand", typeof(ICommand), typeof(SingleDomainObjectBrowser), new UIPropertyMetadata());

      public ICommand SingleDomainObjectBrowser_CreateCommand
      {
         get { return (ICommand)GetValue(SingleDomainObjectBrowser_CreateCommandProperty); }
         set { SetValue(SingleDomainObjectBrowser_CreateCommandProperty, value); }
      }

      public static readonly DependencyProperty SingleDomainObjectBrowser_DeleteCommandProperty =
          DependencyProperty.Register("SingleDomainObjectBrowser_DeleteCommand", typeof(ICommand), typeof(SingleDomainObjectBrowser), new UIPropertyMetadata());

      public ICommand SingleDomainObjectBrowser_DeleteCommand
      {
         get { return (ICommand)GetValue(SingleDomainObjectBrowser_DeleteCommandProperty); }
         set { SetValue(SingleDomainObjectBrowser_DeleteCommandProperty, value); }
      }

      public static readonly DependencyProperty SingleDomainObjectBrowser_AddButtonTextProperty =
          DependencyProperty.Register("SingleDomainObjectBrowser_AddButtonText", typeof(string), typeof(SingleDomainObjectBrowser), new UIPropertyMetadata("Ajouter"));

      public string SingleDomainObjectBrowser_AddButtonText
      {
         get { return (string)GetValue(SingleDomainObjectBrowser_AddButtonTextProperty); }
         set { SetValue(SingleDomainObjectBrowser_AddButtonTextProperty, value); }
      }

      public static readonly DependencyProperty SingleDomainObjectBrowser_DeleteButtonVisibilityProperty =
          DependencyProperty.Register("SingleDomainObjectBrowser_DeleteButtonVisibility", typeof(Visibility), typeof(SingleDomainObjectBrowser), new UIPropertyMetadata(Visibility.Visible));

      public Visibility SingleDomainObjectBrowser_DeleteButtonVisibility
      {
         get { return (Visibility)GetValue(SingleDomainObjectBrowser_DeleteButtonVisibilityProperty); }
         set { SetValue(SingleDomainObjectBrowser_DeleteButtonVisibilityProperty, value); }
      }


      public SingleDomainObjectBrowser()
      {
         InitializeComponent();

         SingleDomainObjectBrowser_ConstraintsContent = new ConstraintsView();
         SingleDomainObjectBrowser_FiltersContent = new TextFilterView();       
      }


      private void MoreActionsPopup_PreviewMouseUp(object sender, MouseButtonEventArgs e)
      {
         MoreActionsPopup.IsOpen = false;
      }
   }
}
