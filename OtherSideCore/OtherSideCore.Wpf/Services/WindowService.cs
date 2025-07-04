using Microsoft.Extensions.DependencyInjection;
using OtherSideCore.Adapter;
using OtherSideCore.Adapter.DomainObjectInteractionViewModel;
using OtherSideCore.Adapter.Services;
using OtherSideCore.Adapter.Views;
using OtherSideCore.Application;
using OtherSideCore.Domain;
using OtherSideCore.Wpf.UserControls.Window;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OtherSideCore.Wpf.Services
{
    public class WindowService : IWindowService
   {
      #region Fields

      protected IServiceProvider _serviceProvider;
      private IViewLocatorService _viewLocatorService;

      private readonly Dictionary<Window, Stack<Border>> _modalPopupStacks;
      private readonly List<Window> _windows;
      private Window _activeWindow;

      protected MainWindow _mainWindow;
      protected MainWindowViewModel _mainWindowViewModel;

      private Type _windowViewModelType;

      private readonly Dictionary<Border, WindowSession> _modalSessions = new();
      private readonly Dictionary<Window, WindowSession> _subWindowSessions = new();

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
         _viewLocatorService = serviceProvider.GetRequiredService<IViewLocatorService>();

         _modalPopupStacks = new Dictionary<Window, Stack<Border>>();
         _windows = new List<Window>();
      }

      #endregion

      #region Public Methods   

      public void ConfigureService(Type windowViewModelType)
      {
         _windowViewModelType = windowViewModelType;
      }

      public void ConfigureMainWindow(object window)
      {
         if (window is MainWindow mainWindow)
         {
            mainWindow.MainWindow_ModalContent = new UserControl();
            mainWindow.MainWindow_ModalContent.Content = new Grid() { Name = "ContentGrid" };

            _modalPopupStacks.Add(mainWindow, new Stack<Border>());

            mainWindow.Activated += OnWindowActivated;
         }
         else
         {
            throw new ArgumentException($"Parameter window must be of type {typeof(MainWindow)}");
         }
      }

      public void HideTopModal()
      {
         var currentModalPopupStack = _modalPopupStacks[_activeWindow];

         if (currentModalPopupStack.Count > 0)
         {
            var modalOverlay = currentModalPopupStack.Peek();

            var @continue = true;

            if (modalOverlay.DataContext is ISavable iSavableContext && iSavableContext.HasUnsavedChanges)
            {
               var res = MessageBox.Show("Abandonner les changements ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

               @continue = res == MessageBoxResult.Yes;
            }

            if (@continue)
            {
               var overlay = currentModalPopupStack.Pop();
               WpfHelper.FindChildByName<Grid>(GetModalContent(_activeWindow), "ContentGrid").Children.Remove(modalOverlay);

               if (_modalSessions.TryGetValue(overlay, out var session))
               {
                  session.NotifyClosed();
                  _modalSessions.Remove(overlay);
               }
            }
         }
      }

      public void CloseWindow(object window)
      {
         _modalPopupStacks.Remove((Window)window);
         ((Window)window).Close();
      }

      public IWindowSession DisplayView(StringKey key, string viewName, object viewModel, DisplayType displayType)
      {
         UserControl view = (UserControl)_viewLocatorService.ResolveView(key, viewModel);

         if (displayType == DisplayType.Modal)
         {
            view.Width = 1200;
            view.Height = 800;
            view.Margin = new Thickness(24, 16, 24, 16);
         }

         var session = new WindowSession();

         ShowView(view, displayType, session);

         return session;
      }

      public void ShowDomainObjectReferenceSelectors(List<DomainObjectReferenceSelectorViewModel> domainObjectReferenceSelectorViewModels, DisplayType displayType)
      {

      }      

      #endregion

      #region Private Methods

      protected void ShowModal(object modalContent, WindowSession windowSession)
      {
         var userControl = (UserControl)modalContent;

         var modalOverlay = new Border
         {
            Background = new SolidColorBrush(Color.FromArgb(120, 0, 0, 0)),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
         };

         _modalSessions[modalOverlay] = windowSession;

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

      protected void ShowSubWindow(object content, WindowSession windowSession)
      {
         if (content is UserControl userControl)
         {
            var subWindow = (SubWindow)ShowSubWindow();
            subWindow.SubWindow_ViewContent = userControl;

            _subWindowSessions[subWindow] = windowSession;

            subWindow.Closed += (_, _) =>
            {
               if (_subWindowSessions.TryGetValue(subWindow, out var session))
               {
                  session.NotifyClosed();
                  _subWindowSessions.Remove(subWindow);
               }
            };
         }
         else
         {
            throw new ArgumentException("Parameter content must be of type UserControl");
         }
      }

      protected void ShowView(object view, DisplayType displayType, WindowSession windowSession)
      {
         if (displayType == DisplayType.SubWindow)
         {
            ShowSubWindow(view, windowSession);
         }
         else if (displayType == DisplayType.Modal)
         {
            ShowModal(view, windowSession);
         }
         else
         {
            throw new ArgumentException("Unknown display type", displayType.ToString());
         }
      }      

      protected object ShowSubWindow()
      {
         var window = CreateSubWindow();

         var windowViewModel = _serviceProvider.GetRequiredService(_windowViewModelType);       
         window.DataContext = windowViewModel;

         window.Show();

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
