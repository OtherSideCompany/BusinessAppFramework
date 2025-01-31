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

namespace OtherSideCore.Wpf.UserControls.Buttons
{
   /// <summary>
   /// Interaction logic for DomainObjectLink.xaml
   /// </summary>
   public partial class DomainObjectLink : UserControl
   {
      public static readonly DependencyProperty DomainObjectLink_LinkClickedCommandProperty =
         DependencyProperty.Register("DomainObjectLink_LinkClickedCommand", typeof(ICommand), typeof(DomainObjectLink), new UIPropertyMetadata(null));

      public ICommand DomainObjectLink_LinkClickedCommand
      {
         get { return (ICommand)GetValue(DomainObjectLink_LinkClickedCommandProperty); }
         set { SetValue(DomainObjectLink_LinkClickedCommandProperty, value); }
      }

      public static readonly DependencyProperty DomainObjectLink_IdProperty =
         DependencyProperty.Register("DomainObjectLink_Id", typeof(int), typeof(DomainObjectLink), new UIPropertyMetadata(0));

      public int DomainObjectLink_Id
      {
         get { return (int)GetValue(DomainObjectLink_IdProperty); }
         set { SetValue(DomainObjectLink_IdProperty, value); }
      }

      public static readonly DependencyProperty DomainObjectLink_NameProperty =
         DependencyProperty.Register("DomainObjectLink_Name", typeof(string), typeof(DomainObjectLink), new UIPropertyMetadata("-NA-"));

      public string DomainObjectLink_Name
      {
         get { return (string)GetValue(DomainObjectLink_NameProperty); }
         set { SetValue(DomainObjectLink_NameProperty, value); }
      }

      public DomainObjectLink()
      {
         InitializeComponent();
      }
   }
}
