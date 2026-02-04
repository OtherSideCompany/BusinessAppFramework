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
   /// Interaction logic for DomainObjectReferencesEditor.xaml
   /// </summary>
   public partial class DomainObjectReferencesEditor : UserControl
   {
      public static readonly DependencyProperty DomainObjectReferencesEditor_DataTemplateSelectorProperty =
          DependencyProperty.Register(nameof(DomainObjectReferencesEditor_DataTemplateSelector), typeof(System.Windows.Controls.DataTemplateSelector), typeof(DomainObjectReferencesEditor), new UIPropertyMetadata());

      public System.Windows.Controls.DataTemplateSelector DomainObjectReferencesEditor_DataTemplateSelector
      {
         get { return (System.Windows.Controls.DataTemplateSelector)GetValue(DomainObjectReferencesEditor_DataTemplateSelectorProperty); }
         set { SetValue(DomainObjectReferencesEditor_DataTemplateSelectorProperty, value); }
      }
      public DomainObjectReferencesEditor()
      {
         InitializeComponent();
      }
   }
}
