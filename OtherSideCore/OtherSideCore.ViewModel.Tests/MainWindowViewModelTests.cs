using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using OtherSideCore.Domain.ModelObjects;
using OtherSideCore.Domain.Services;
using System;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;

namespace OtherSideCore.ViewModel.Tests
{
   public class MainWindowViewModelTests
   {
      private MainWindowViewModel _mainWindowViewModel;
      private ViewDescription _view1;
      private ViewDescription _view2;
      private DashboardDescription _viewGroup;

      public MainWindowViewModelTests()
      {
         var authenticationService = new Mock<IAuthenticationService>();
         var serviceCollection = new ServiceCollection();
         serviceCollection.AddTransient<DefaultViewModel>();
         var serviceProvider = serviceCollection.BuildServiceProvider();
         var loggerFactory = new Mock<ILoggerFactory>();
         var loggerMock = new Mock<ILogger>();
         loggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(loggerMock.Object);

         _mainWindowViewModel = new CustomMainWindowViewModel(authenticationService.Object, serviceProvider, loggerFactory.Object);

         _view1 = new ViewDescription(serviceProvider, loggerFactory.Object, "view 1", typeof(DefaultViewModel), null);
         _view2 = new ViewDescription(serviceProvider, loggerFactory.Object, "view 2", typeof(DefaultViewModel), null);
         _viewGroup = new DashboardDescription(serviceProvider, loggerFactory.Object, "view group", typeof(DefaultDashboardViewModel), null);

         _viewGroup.SubViewDescriptions.Add(_view1);
         _viewGroup.SubViewDescriptions.Add(_view2);

         _mainWindowViewModel.ViewDescriptions.Add(_view1);
         _mainWindowViewModel.ViewDescriptions.Add(_view2);
      }

      [Fact]
      public void DisplayView_ViewIsDisplayed()
      {
         if (_mainWindowViewModel.DisplayViewCommand.CanExecute(_view1))
         {
            _mainWindowViewModel.DisplayViewCommand.Execute(_view1);
         }

         Assert.NotNull(_mainWindowViewModel.LoadedViewDescription);
         Assert.True(_view1.IsLoaded);
         Assert.False(_view2.IsLoaded);
         Assert.NotNull(_view1.ViewViewModelBase);
         Assert.Null(_view2.ViewViewModelBase);

         if (_mainWindowViewModel.DisplayViewCommand.CanExecute(_view2))
         {
            _mainWindowViewModel.DisplayViewCommand.Execute(_view2);
         }

         Assert.False(_view1.IsLoaded);
         Assert.True(_view2.IsLoaded);
         Assert.Null(_view1.ViewViewModelBase);
         Assert.NotNull(_view2.ViewViewModelBase);
      }

      [Fact]
      public void DisplayView_ViewGroupHasSubViewLoaded()
      {
         if (_mainWindowViewModel.DisplayViewCommand.CanExecute(_view1))
         {
            _mainWindowViewModel.DisplayViewCommand.Execute(_view1);
         }

         Assert.True(_viewGroup.IsSubViewLoaded);
      }

      private class CustomMainWindowViewModel : MainWindowViewModel
      {
         public CustomMainWindowViewModel(IAuthenticationService authenticationService, IServiceProvider serviceProvider, ILoggerFactory loggerFactory) : base(authenticationService, serviceProvider, loggerFactory)
         {
         }
      }

      private class DefaultViewModel : ViewViewModelBase
      {
         public override void Dispose()
         {
            
         }
      }

      private class DefaultDashboardViewModel : DashboardViewModelBase
      {
         public override void Dispose()
         {

         }
      }
   }
}