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
      private MainWindowViewModel<User> _mainWindowViewModel;
      private View _view1;
      private View _view2;
      private ViewGroup _viewGroup;

      public MainWindowViewModelTests()
      {
         var authenticationService = new Mock<IAuthenticationService<User>>();
         var serviceCollection = new ServiceCollection();
         serviceCollection.AddTransient<DefaultViewModel>();
         var serviceProvider = serviceCollection.BuildServiceProvider();
         var loggerFactory = new Mock<ILoggerFactory>();
         var loggerMock = new Mock<ILogger>();
         loggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(loggerMock.Object);

         _mainWindowViewModel = new CustomMainWindowViewModel(authenticationService.Object, serviceProvider, loggerFactory.Object);

         _view1 = new View(serviceProvider, loggerFactory.Object, "view 1", typeof(DefaultViewModel), null);
         _view2 = new View(serviceProvider, loggerFactory.Object, "view 2", typeof(DefaultViewModel), null);
         _viewGroup = new ViewGroup(serviceProvider, loggerFactory.Object, "view group", null);

         _viewGroup.SubViews.Add(_view1);
         _viewGroup.SubViews.Add(_view2);

         _mainWindowViewModel.Views.Add(_view1);
         _mainWindowViewModel.Views.Add(_view2);
      }

      [Fact]
      public void DisplayView_ViewIsDisplayed()
      {
         if (_mainWindowViewModel.DisplayViewCommand.CanExecute(_view1))
         {
            _mainWindowViewModel.DisplayViewCommand.Execute(_view1);
         }

         Assert.NotNull(_mainWindowViewModel.LoadedView);
         Assert.True(_view1.IsLoaded);
         Assert.False(_view2.IsLoaded);
         Assert.NotNull(_view1.ViewModel);
         Assert.Null(_view2.ViewModel);

         if (_mainWindowViewModel.DisplayViewCommand.CanExecute(_view2))
         {
            _mainWindowViewModel.DisplayViewCommand.Execute(_view2);
         }

         Assert.False(_view1.IsLoaded);
         Assert.True(_view2.IsLoaded);
         Assert.Null(_view1.ViewModel);
         Assert.NotNull(_view2.ViewModel);
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

      private class CustomMainWindowViewModel : MainWindowViewModel<User>
      {
         public CustomMainWindowViewModel(IAuthenticationService<User> authenticationService, IServiceProvider serviceProvider, ILoggerFactory loggerFactory) : base(authenticationService, serviceProvider, loggerFactory)
         {
         }
      }

      private class DefaultViewModel : ViewModelBase
      {
         public override void Dispose()
         {
            
         }
      }
   }
}