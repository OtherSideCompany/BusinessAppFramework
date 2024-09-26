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
using System.Windows.Shapes;

namespace OtherSideCore.Wpf.UserControls
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
      #region Fields

      private readonly Stack<Border> _modalPopupStack;

      #endregion

      #region Properties

      public static readonly DependencyProperty MainWindow_ApplicationLogoColorProperty =
          DependencyProperty.Register("MainWindow_ApplicationLogoColor", typeof(SolidColorBrush), typeof(MainWindow), new UIPropertyMetadata(Brushes.Blue));

      public SolidColorBrush MainWindow_ApplicationLogoColor
      {
         get { return (SolidColorBrush)GetValue(MainWindow_ApplicationLogoColorProperty); }
         set { SetValue(MainWindow_ApplicationLogoColorProperty, value); }
      }

      #endregion

      #region Constructor

      public MainWindow()
      {
         InitializeComponent();

         _modalPopupStack = new Stack<Border>();
      }

      #endregion

      #region Public Methods

      public void ShowModal(UserControl modalContent)
      {
         var modalOverlay = new Border
         {
            Background = new SolidColorBrush(Color.FromArgb(120, 0, 0, 0)),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
         };

         modalOverlay.MouseDown += (sender, e) => HideModal();

         var modalPopupBorder = new ModalPopupBorder();
         modalPopupBorder.VerticalAlignment = VerticalAlignment.Center;
         modalPopupBorder.HorizontalAlignment = HorizontalAlignment.Center;
         modalPopupBorder.ContentBorder.Child = modalContent;
         modalPopupBorder.DataContext = modalContent.DataContext;

         modalOverlay.Child = new ContentControl { Content = modalPopupBorder };
         modalOverlay.DataContext = modalContent.DataContext;

         ModalPopupHostGrid.Children.Add(modalOverlay);
         Panel.SetZIndex(modalOverlay, 100 + _modalPopupStack.Count);
         _modalPopupStack.Push(modalOverlay);
      }

      public void HideModal()
      {
         if (_modalPopupStack.Count > 0)
         {
            var modalOverlay = _modalPopupStack.Peek();
            var @continue = true;

            if (((ViewViewModelBase)modalOverlay.DataContext).HasUnsavedChanges())
            {
               var res = MessageBox.Show("Abandonner les changements ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

               @continue = res == MessageBoxResult.Yes;
            }

            if (@continue)
            {
               _modalPopupStack.Pop();
               ((ViewViewModelBase)modalOverlay.DataContext).Dispose();
               ModalPopupHostGrid.Children.Remove(modalOverlay);
            }
         }
      }

      #endregion
   }
}
