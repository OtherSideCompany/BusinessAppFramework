using OtherSideCore.Wpf.UserControls;
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
   /// Interaction logic for DomainObjectReferenceView.xaml
   /// </summary>
   public partial class DomainObjectReferenceView : UserControl
   {
      public static readonly DependencyProperty DomainObjectReferenceView_DeleteReferenceCommandProperty =
          DependencyProperty.Register("DomainObjectReferenceView_DeleteReferenceCommand", typeof(ICommand), typeof(DomainObjectReferenceView), new UIPropertyMetadata());

      public ICommand DomainObjectReferenceView_DeleteReferenceCommand
      {
         get { return (ICommand)GetValue(DomainObjectReferenceView_DeleteReferenceCommandProperty); }
         set { SetValue(DomainObjectReferenceView_DeleteReferenceCommandProperty, value); }
      }

      public static readonly DependencyProperty DomainObjectReferenceView_DataTemplateSelectorProperty =
          DependencyProperty.Register("DomainObjectReferenceView_DataTemplateSelector", typeof(System.Windows.Controls.DataTemplateSelector), typeof(DomainObjectReferenceView), new UIPropertyMetadata());

      public ICommand DomainObjectReferenceView_DataTemplateSelector
      {
         get { return (ICommand)GetValue(DomainObjectReferenceView_DataTemplateSelectorProperty); }
         set { SetValue(DomainObjectReferenceView_DataTemplateSelectorProperty, value); }
      }

      public DomainObjectReferenceView()
      {
         InitializeComponent();
      }
   }
}
