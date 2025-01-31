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

namespace OtherSideCore.Wpf.UserControls.Editor
{
   /// <summary>
   /// Interaction logic for DomainObjectReferenceSelectorsView.xaml
   /// </summary>
   public partial class DomainObjectReferenceSelectorsView : UserControl
   {
      public static readonly DependencyProperty DomainObjectReferenceSelectorsView_DataTemplateSelectorProperty =
          DependencyProperty.Register("DomainObjectReferenceSelectorsView_DataTemplateSelector", typeof(System.Windows.Controls.DataTemplateSelector), typeof(DomainObjectReferenceSelectorsView), new UIPropertyMetadata());

      public System.Windows.Controls.DataTemplateSelector DomainObjectReferenceSelectorsView_DataTemplateSelector
      {
         get { return (System.Windows.Controls.DataTemplateSelector)GetValue(DomainObjectReferenceSelectorsView_DataTemplateSelectorProperty); }
         set { SetValue(DomainObjectReferenceSelectorsView_DataTemplateSelectorProperty, value); }
      }

      public DomainObjectReferenceSelectorsView()
      {
         InitializeComponent();
      }
   }
}
