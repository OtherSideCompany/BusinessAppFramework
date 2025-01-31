using Microsoft.Extensions.DependencyInjection;
using OtherSideCore.Adapter;
using OtherSideCore.Adapter.DomainObjectInteraction;
using OtherSideCore.Adapter.DomainObjectInteractionViewModel;
using OtherSideCore.Adapter.Factories;
using OtherSideCore.Adapter.ViewDescriptions;
using OtherSideCore.Adapter.Views;
using OtherSideCore.Wpf.UserControls;
using OtherSideCore.Wpf.UserControls.Window;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OtherSideCore.Wpf.Services
{
   public abstract class WindowService : IWindowService
   {
      #region Fields

      protected IServiceProvider _serviceProvider;
      protected IModuleViewFactory _viewFactory;

      private readonly Dictionary<Window, Stack<Border>> _modalPopupStacks;
      private readonly List<Window> _windows;
      private Window _activeWindow;

      protected MainWindow _mainWindow;
      protected MainWindowViewModel _mainWindowViewModel;

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public WindowService(IServiceProvider serviceProvider, IModuleViewFactory viewFactory)
      {
         System.Windows.Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

         _serviceProvider = serviceProvider;
         _viewFactory = viewFactory;

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

      public void ShowSubWindow(object content, string windowName)
      {
         if (content is UserControl userControl)
         {
            var subWindow = (SubWindow)ShowSubWindow();
            subWindow.SubWindow_ViewContent = userControl;

            ((WindowViewModel)subWindow.DataContext).WindowName = windowName;
         }
         else
         {
            throw new ArgumentException("Parameter content must be of type UserControl");
         }
      }

      public void ShowView(object view, string windowName, DisplayType displayType)
      {
         if (displayType == DisplayType.SubWindow)
         {
            ShowSubWindow(view, windowName);
         }
         else if (displayType == DisplayType.Modal)
         {
            ShowModal(view);
         }
         else
         {
            throw new ArgumentException("Unknown display type", displayType.ToString());
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
               currentModalPopupStack.Pop();
               WpfHelper.FindChildByName<Grid>(GetModalContent(_activeWindow), "ContentGrid").Children.Remove(modalOverlay);
            }
         }
      }
      public void ShowMainWindow<T>() where T : MainWindowViewModel
      {
         _mainWindow = CreateMainWindow();

         _mainWindowViewModel = _serviceProvider.GetRequiredService<T>();

         _mainWindowViewModel.LoadedViewModelChanged += MainWindowViewModel_LoadedViewModelChanged;

         SetWindowViewModelDefaultProperties(_mainWindowViewModel);
         BuildModulesAndWorkspaceDescriptions(_mainWindowViewModel);

         _mainWindow.DataContext = _mainWindowViewModel;
         _mainWindow.Show();
      }

      public void CloseWindow(object window)
      {
         _modalPopupStacks.Remove((Window)window);
         ((Window)window).Close();
      }

      public abstract void ShowDomainObjectReferenceSelectors(List<DomainObjectReferenceSelectorViewModel> domainObjectReferenceSelectorViewModels, DisplayType displayType);
      public abstract void ShowDomainObjectSearchView(DomainObjectViewModel domainObjectViewModel, WorkspaceViewModel workspaceViewModel, DisplayType displayType);
      public abstract void ShowDomainObjectSearchView(Type domainObjectType, WorkspaceViewModel workspaceViewModel, DisplayType displayType);
      public abstract void ShowDomainObjectEditorView(IDomainObjectEditorViewModel editorViewModel, DisplayType displayType);
      public abstract Task ShowDomainObjectSelectorViewAsync(IDomainObjectSelectorViewModel domainObjectSelectorViewModel, DisplayType displayType);
      public abstract void ShowDomainObjectTreeViewWorkspace(DomainObjectTreeViewModel domainObjectTreeViewModel, Type domainObjectType, DisplayType displayType);
      public abstract ViewDescriptionBase GetDescription(ViewBaseViewModel viewBaseViewModel);            

      #endregion

      #region Private Methods

      private object ShowSubWindow()
      {
         var window = CreateSubWindow();

         var windowViewModel = _serviceProvider.GetRequiredService<WindowViewModel>();

         SetWindowViewModelDefaultProperties(windowViewModel);

         window.DataContext = windowViewModel;

         window.Show();

         return window;
      }     

      private void MainWindowViewModel_LoadedViewModelChanged(object? sender, EventArgs e)
      {
         if (_mainWindowViewModel.LoadedViewViewModel is ModuleViewModel moduleViewModel)
         {
            _mainWindow.MainWindow_ViewContent = (UserControl)_viewFactory.CreateView(moduleViewModel.ModuleDescription);
         }
         else if (_mainWindowViewModel.LoadedViewViewModel is WorkspaceViewModel workspaceViewModel)
         {
            _mainWindow.MainWindow_ViewContent = (UserControl)_viewFactory.CreateView(workspaceViewModel.WorkspaceDescription);
         }

         _mainWindow.MainWindow_ViewContent.DataContext = _mainWindowViewModel.LoadedViewViewModel;
      }

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

      protected abstract void SetWindowViewModelDefaultProperties(WindowViewModel windowViewModel);

      protected abstract void BuildModulesAndWorkspaceDescriptions(MainWindowViewModel mainWindowViewModel);

      #endregion
   }
}
