using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using OtherSideCore.Domain.ModelObjects;
using OtherSideCore.Domain.Repositories;
using OtherSideCore.Domain.Services;
using System;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;

namespace OtherSideCore.ViewModel.Tests
{
   public class MainWindowViewModelTests
   {
      private DefaultMainWindowViewModel _mainWindowViewModel;
      private ViewDescription _view1;
      private ViewDescription _view2;
      private DashboardDescription _viewGroup;

      public MainWindowViewModelTests()
      {
         var loggerFactory = new Mock<ILoggerFactory>();
         var loggerMock = new Mock<ILogger>();
         loggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(loggerMock.Object);

         var serviceCollection = new ServiceCollection();
         serviceCollection.AddTransient<DefaultViewModel>();
         serviceCollection.AddSingleton<DefaultMainWindowViewModel>();
         serviceCollection.AddSingleton<IAuthenticationService>(new Mock<IAuthenticationService>().Object);
         serviceCollection.AddSingleton<IRepositoryFactory>(new Mock<IRepositoryFactory>().Object);
         serviceCollection.AddSingleton<ILoggerFactory>(loggerFactory.Object);
         serviceCollection.AddSingleton<IGlobalDataService>(new Mock<IGlobalDataService>().Object);
         serviceCollection.AddSingleton<IModelObjectViewModelFactory>(new Mock<IModelObjectViewModelFactory>().Object);

         var serviceProvider = serviceCollection.BuildServiceProvider();         

         _mainWindowViewModel = serviceProvider.GetRequiredService<DefaultMainWindowViewModel>();

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
         if (_mainWindowViewModel.DisplayViewAsyncCommand.CanExecute(_view1))
         {
            _mainWindowViewModel.DisplayViewAsyncCommand.Execute(_view1);
         }

         Assert.NotNull(_mainWindowViewModel.LoadedViewDescription);
         Assert.True(_view1.IsLoaded);
         Assert.False(_view2.IsLoaded);
         Assert.NotNull(_view1.ViewViewModelBase);
         Assert.Null(_view2.ViewViewModelBase);

         if (_mainWindowViewModel.DisplayViewAsyncCommand.CanExecute(_view2))
         {
            _mainWindowViewModel.DisplayViewAsyncCommand.Execute(_view2);
         }

         Assert.False(_view1.IsLoaded);
         Assert.True(_view2.IsLoaded);
         Assert.Null(_view1.ViewViewModelBase);
         Assert.NotNull(_view2.ViewViewModelBase);
      }

      [Fact]
      public void DisplayView_ViewGroupHasSubViewLoaded()
      {
         if (_mainWindowViewModel.DisplayViewAsyncCommand.CanExecute(_view1))
         {
            _mainWindowViewModel.DisplayViewAsyncCommand.Execute(_view1);
         }

         Assert.True(_viewGroup.IsSubViewLoaded);
      }

      private class CustomMainWindowViewModel : MainWindowViewModel
      {
         public CustomMainWindowViewModel(IAuthenticationService authenticationService, IServiceProvider serviceProvider, IRepositoryFactory repository, ILoggerFactory loggerFactory, IGlobalDataService globalDataService) : base(authenticationService, serviceProvider, loggerFactory, repository, globalDataService)
         {
         }
      }

      private class DefaultViewModel : ViewViewModelBase
      {
         public DefaultViewModel(IAuthenticationService authenticationService, IRepositoryFactory repositoryFactory, IModelObjectViewModelFactory modelObjectViewModeFactory, ILoggerFactory loggerFactory, IGlobalDataService globalDataService, IModelObjectFactory modelObjectFactory) : base(authenticationService, repositoryFactory, modelObjectViewModeFactory, loggerFactory, globalDataService, modelObjectFactory)
         {
         }

         public override void Dispose()
         {
            
         }

         public override bool HasUnsavedChanges()
         {
            return false;
         }

         public override async Task InitializeAsync(CancellationToken cancellationToken)
         {
            await Task.Delay(0);
         }
      }

      private class DefaultDashboardViewModel : DashboardViewModelBase
      {
         public DefaultDashboardViewModel(IAuthenticationService authenticationService, IRepositoryFactory repositoryFactory, IModelObjectViewModelFactory modelObjectViewModeFactory, ILoggerFactory loggerFactory, IGlobalDataService globalDataService, IModelObjectFactory modelObjectFactory) : base(authenticationService, repositoryFactory, modelObjectViewModeFactory, loggerFactory, globalDataService, modelObjectFactory)
         {
         }

         public override void Dispose()
         {

         }

         public override async Task InitializeAsync(CancellationToken cancellationToken)
         {
            await Task.Delay(0);
         }
      }

      private class DefaultMainWindowViewModel : MainWindowViewModel
      {
         public DefaultMainWindowViewModel(IAuthenticationService authenticationService, IServiceProvider serviceProvider, ILoggerFactory loggerFactory, IRepositoryFactory repositoryFactory, IGlobalDataService globalDataService) : base(authenticationService, serviceProvider, loggerFactory, repositoryFactory, globalDataService)
         {
         }
      }
   }
}