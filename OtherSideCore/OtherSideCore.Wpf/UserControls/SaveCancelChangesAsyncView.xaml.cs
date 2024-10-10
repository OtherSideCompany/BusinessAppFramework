using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OtherSideCore.Wpf.UserControls
{
   /// <summary>
   /// Interaction logic for SaveCancelChangesAsyncView.xaml
   /// </summary>
   public partial class SaveCancelChangesAsyncView : UserControl
   {
      public static readonly DependencyProperty SaveCancelChangesAsyncView_SaveCommandProperty =
          DependencyProperty.Register("SaveCancelChangesAsyncView_SaveCommand", typeof(ICommand), typeof(SaveCancelChangesAsyncView), new UIPropertyMetadata(null));

      public ICommand SaveCancelChangesAsyncView_SaveCommand
      {
         get { return (ICommand)GetValue(SaveCancelChangesAsyncView_SaveCommandProperty); }
         set { SetValue(SaveCancelChangesAsyncView_SaveCommandProperty, value); }
      }

      public static readonly DependencyProperty SaveCancelChangesAsyncView_CancelCommandProperty =
          DependencyProperty.Register("SaveCancelChangesAsyncView_CancelCommand", typeof(ICommand), typeof(SaveCancelChangesAsyncView), new UIPropertyMetadata(null));

      public ICommand SaveCancelChangesAsyncView_CancelCommand
      {
         get { return (ICommand)GetValue(SaveCancelChangesAsyncView_CancelCommandProperty); }
         set { SetValue(SaveCancelChangesAsyncView_CancelCommandProperty, value); }
      }

      public SaveCancelChangesAsyncView()
      {
         InitializeComponent();
      }
   }
}
