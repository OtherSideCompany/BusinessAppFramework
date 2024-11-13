using Microsoft.Extensions.DependencyInjection;
using OtherSideCore.Adapter;
using OtherSideCore.Adapter.Views;
using OtherSideCore.Wpf.UserControls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OtherSideCore.Wpf.Services
{
   public abstract class WindowService : IWindowService
   {
      #region Fields

      protected IServiceProvider _serviceProvider;

      private readonly Dictionary<Window, Stack<Border>> _modalPopupStacks;
      private readonly List<Window> _windows;
      private Window _activeWindow;

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public WindowService(IServiceProvider serviceProvider)
      {
         System.Windows.Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

         _serviceProvider = serviceProvider;

         _modalPopupStacks = new Dictionary<Window, Stack<Border>>();
         _windows = new List<Window>();
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

         WpfHelper.FindChildByName<Grid>(GetModalContent(_activeWindow), "ContentGrid").Children.Add(modalOverlay);
         Panel.SetZIndex(modalOverlay, 100 + _modalPopupStacks[_activeWindow].Count);
         _modalPopupStacks[_activeWindow].Push(modalOverlay);
      }

      public void HideTopModal()
      {
         var currentModalPopupStack = _modalPopupStacks[_activeWindow];

         if (currentModalPopupStack.Count > 0)
         {
            var modalOverlay = currentModalPopupStack.Peek();

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
               currentModalPopupStack.Pop();
               WpfHelper.FindChildByName<Grid>(GetModalContent(_activeWindow), "ContentGrid").Children.Remove(modalOverlay);
            }
         }
      }

      public abstract void ShowSubWindow(object content);

      public abstract void ShowMainWindow();

      public void CloseWindow(object window)
      {
         _modalPopupStacks.Remove((Window)window);
         ((Window)window).Close(); 
      }

      #endregion

      #region Private Methods

      protected MainWindow CreateMainWindow()
      {
         var window = _serviceProvider.GetRequiredService<MainWindow>();
         window.MainWindow_ModalContent = new UserControl();
         window.MainWindow_ModalContent.Content = new Grid() { Name = "ContentGrid" };
         
         _modalPopupStacks.Add(window, new Stack<Border>());

         window.Activated += OnWindowActivated;

         return window;
      }

      protected SubWindow CreateSubWindow()
      {
         var window = _serviceProvider.GetRequiredService<SubWindow>();
         window.SubWindow_ModalContent = new UserControl();
         window.SubWindow_ModalContent.Content = new Grid() { Name = "ContentGrid" };

         _modalPopupStacks.Add(window, new Stack<Border>());

         window.Activated += OnWindowActivated;

         return window;
      }

      private void OnWindowActivated(object sender, EventArgs e)
      {
         _activeWindow = sender as Window;
      }

      private UserControl GetModalContent(Window window)
      {
         if (window is MainWindow mainWindow)
         {
            return mainWindow.MainWindow_ModalContent;
         }
         else if (window is SubWindow subWindow)
         {
            return subWindow.SubWindow_ModalContent;
         }
         else
         {
            return null;
         }
      }

      #endregion
   }
}
