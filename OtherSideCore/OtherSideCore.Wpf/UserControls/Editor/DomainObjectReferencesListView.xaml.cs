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
   /// Interaction logic for DomainObjectReferencesListView.xaml
   /// </summary>
   public partial class DomainObjectReferencesListView : UserControl
   {
      public static readonly DependencyProperty DomainObjectReferencesListView_ShowReferenceEditorCommandProperty =
          DependencyProperty.Register("DomainObjectReferencesListView_ShowReferenceEditorCommand", typeof(ICommand), typeof(DomainObjectReferencesListView), new UIPropertyMetadata());

      public ICommand DomainObjectReferencesListView_ShowReferenceEditorCommand
      {
         get { return (ICommand)GetValue(DomainObjectReferencesListView_ShowReferenceEditorCommandProperty); }
         set { SetValue(DomainObjectReferencesListView_ShowReferenceEditorCommandProperty, value); }
      }

      public static readonly DependencyProperty DomainObjectReferencesListView_DataTemplateSelectorProperty =
          DependencyProperty.Register("DomainObjectReferencesListView_DataTemplateSelector", typeof(System.Windows.Controls.DataTemplateSelector), typeof(DomainObjectReferencesListView), new UIPropertyMetadata());

      public System.Windows.Controls.DataTemplateSelector DomainObjectReferencesListView_DataTemplateSelector
      {
         get { return (System.Windows.Controls.DataTemplateSelector)GetValue(DomainObjectReferencesListView_DataTemplateSelectorProperty); }
         set { SetValue(DomainObjectReferencesListView_DataTemplateSelectorProperty, value); }
      }
      public DomainObjectReferencesListView()
      {
         InitializeComponent();
      }
   }
}
