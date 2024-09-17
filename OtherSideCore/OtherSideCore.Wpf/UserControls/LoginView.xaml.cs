using OtherSideCore.ViewModel;
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
   /// Interaction logic for LoginView.xaml
   /// </summary>
   public partial class LoginView : UserControl
   {
      public static readonly DependencyProperty LoginView_ConnectionButtonColorProperty =
          DependencyProperty.Register("LoginView_ConnectionButtonColor", typeof(SolidColorBrush), typeof(LoginView), new UIPropertyMetadata(Brushes.Blue));

      public SolidColorBrush LoginView_ConnectionButtonColor
      {
         get { return (SolidColorBrush)GetValue(LoginView_ConnectionButtonColorProperty); }
         set { SetValue(LoginView_ConnectionButtonColorProperty, value); }
      }

      public LoginView()
      {
         InitializeComponent();

         this.Loaded += ConnexionView_Loaded;
      }

      public void SetFocusOnInputFields()
      {
         if (string.IsNullOrEmpty(UserNameTextBox.Text))
         {
            Keyboard.Focus(UserNameTextBox);
         }
         else
         {
            Keyboard.Focus(PasswordTextBox);
         }
      }

      private void ConnexionView_Loaded(object sender, RoutedEventArgs e)
      {
         SetFocusOnInputFields();
      }

      private void ConnectionButton_Click(object sender, RoutedEventArgs e)
      {
         SetConnexionPassword();
      }

      private void OnKeyDownHandler(object sender, KeyEventArgs e)
      {
         if (e.Key == Key.Return)
         {
            SetConnexionPassword();
            ((MainWindowViewModel)DataContext).AuthenticateUserAsyncCommand.Execute(null);
         }
      }

      private void SetConnexionPassword()
      {
         ((MainWindowViewModel)DataContext).ConnexionPassword = PasswordTextBox.Password;
         PasswordTextBox.Password = "";
      }
   }
}
