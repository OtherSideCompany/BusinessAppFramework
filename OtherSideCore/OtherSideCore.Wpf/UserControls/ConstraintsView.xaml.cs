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

namespace OtherSideCore.Wpf.UserControls
{
   /// <summary>
   /// Interaction logic for ConstraintsView.xaml
   /// </summary>
   public partial class ConstraintsView : UserControl
   {
      public static readonly DependencyProperty ConstraintsView_SearchCommandProperty =
            DependencyProperty.Register("ConstraintsView_SearchCommand", typeof(ICommand), typeof(ConstraintsView), new UIPropertyMetadata(null));

      public ICommand ConstraintsView_SearchCommand
      {
         get { return (ICommand)GetValue(ConstraintsView_SearchCommandProperty); }
         set { SetValue(ConstraintsView_SearchCommandProperty, value); }
      }

      public ConstraintsView()
      {
         InitializeComponent();
      }
   }
}
