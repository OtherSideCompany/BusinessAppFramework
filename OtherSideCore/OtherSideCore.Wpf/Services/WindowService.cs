using OtherSideCore.Adapter;
using OtherSideCore.Adapter.Views;
using OtherSideCore.Wpf.UserControls;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OtherSideCore.Wpf.Services
{
   public class WindowService : IWindowService
   {
      #region Fields

      private readonly Stack<Border> _modalPopupStack;
      private UserControl _modalPopupHost;
      private Grid _modalPopupHostGrid;

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public WindowService()
      {
         _modalPopupStack = new Stack<Border>();

         var mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;
         var modalPopupHost = mainWindow.MainWindow_ModalContent;
         _modalPopupHost = modalPopupHost;
         _modalPopupHostGrid = new Grid();
         _modalPopupHost.Content = _modalPopupHostGrid;
      }  

      #endregion

      #region Public Methods

      public void ShowModal(object modalContent)
      {
         var userControl = (UserControl)modalContent;

         var modalOverlay = new Border
         {
            Background = new SolidColorBrush(Color.FromArgb(120, 0, 0, 0)),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
         };

         modalOverlay.MouseDown += (sender, e) => { if (e.OriginalSource == modalOverlay) HideTopModal(); };

         var modalPopupBorder = new ModalPopupBorder();
         modalPopupBorder.VerticalAlignment = VerticalAlignment.Center;
         modalPopupBorder.HorizontalAlignment = HorizontalAlignment.Center;
         modalPopupBorder.ContentBorder.Child = userControl;
         modalPopupBorder.DataContext = userControl.DataContext;

         modalOverlay.Child = new ContentControl { Content = modalPopupBorder };
         modalOverlay.DataContext = userControl.DataContext;

         _modalPopupHostGrid.Children.Add(modalOverlay);
         Panel.SetZIndex(modalOverlay, 100 + _modalPopupStack.Count);
         _modalPopupStack.Push(modalOverlay);
      }

      public void HideTopModal()
      {
         if (_modalPopupStack.Count > 0)
         {
            var modalOverlay = _modalPopupStack.Peek();

            var @continue = true;

            if (modalOverlay.DataContext is WorkspaceViewModel)
            {
               if (((WorkspaceViewModel)modalOverlay.DataContext).Workspace.HasUnsavedChanges)
               {
                  var res = MessageBox.Show("Abandonner les changements ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                  @continue = res == MessageBoxResult.Yes;
               }
            }

            if (@continue)
            {
               _modalPopupStack.Pop();
               _modalPopupHostGrid.Children.Remove(modalOverlay);
            }
         }
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
