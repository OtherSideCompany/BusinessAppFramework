using OtherSideCore.Wpf.UserControls.Browser;
using OtherSideCore.Wpf.UserControls.Filters;
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

namespace OtherSideCore.Wpf.UserControls.Selector
{
   /// <summary>
   /// Interaction logic for DomainObjectSelector.xaml
   /// </summary>
   public partial class DomainObjectSelector : UserControl
   {
      public static readonly DependencyProperty DomainObjectSelector_NameProperty =
          DependencyProperty.Register("DomainObjectSelector_Name", typeof(string), typeof(DomainObjectSelector), new UIPropertyMetadata("Unknown"));

      public string DomainObjectSelector_Name
      {
         get { return (string)GetValue(DomainObjectSelector_NameProperty); }
         set { SetValue(DomainObjectSelector_NameProperty, value); }
      }

      public static readonly DependencyProperty DomainObjectSelector_ConstraintsContentProperty =
          DependencyProperty.Register("DomainObjectSelector_ConstraintsContent", typeof(object), typeof(DomainObjectSelector), new UIPropertyMetadata());

      public object DomainObjectSelector_ConstraintsContent
      {
         get { return GetValue(DomainObjectSelector_ConstraintsContentProperty); }
         set { SetValue(DomainObjectSelector_ConstraintsContentProperty, value); }
      }

      public static readonly DependencyProperty DomainObjectSelector_FiltersContentProperty =
          DependencyProperty.Register("DomainObjectSelector_FiltersContent", typeof(object), typeof(DomainObjectSelector), new UIPropertyMetadata());

      public object DomainObjectSelector_FiltersContent
      {
         get { return GetValue(DomainObjectSelector_FiltersContentProperty); }
         set { SetValue(DomainObjectSelector_FiltersContentProperty, value); }
      }

      public static readonly DependencyProperty DomainObjectSelector_ListContentProperty =
          DependencyProperty.Register("DomainObjectSelector_ListContent", typeof(object), typeof(DomainObjectSelector), new UIPropertyMetadata());

      public object DomainObjectSelector_ListContent
      {
         get { return GetValue(DomainObjectSelector_ListContentProperty); }
         set { SetValue(DomainObjectSelector_ListContentProperty, value); }
      }

      public DomainObjectSelector()
      {
         InitializeComponent();

         DomainObjectSelector_ConstraintsContent = new ConstraintsView();
         DomainObjectSelector_FiltersContent = new TextFilterView();
      }
   }
}
