using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OtherSideCore.Wpf.UserControls
{
   /// <summary>
   /// Interaction logic for ConstraintsView.xaml
   /// </summary>
   public partial class ConstraintsView : UserControl
   {
      public string UniqueGroupName { get; } = Guid.NewGuid().ToString();

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
