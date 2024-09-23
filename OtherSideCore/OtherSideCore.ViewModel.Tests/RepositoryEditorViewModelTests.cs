using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using OtherSideCore.Domain.ModelObjects;
using OtherSideCore.Domain.Repositories;
using OtherSideCore.Domain.Tests.Repositories;
using OtherSideCore.Infrastructure.Entities;
using OtherSideCore.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.ViewModel.Tests
{
   public class RepositoryEditorViewModelTests
   {
      private RepositoryEditorViewModel<DefaultModelObject> _repositoryEditorViewModel;

      public RepositoryEditorViewModelTests()
      {
         var modelObjectFactory = new ModelObjectFactory();

         var loggerFactory = new Mock<ILoggerFactory>();
         var loggerMock = new Mock<ILogger>();
         loggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(loggerMock.Object);

         var dbContextFactory = new Mock<IDbContextFactory<DbContext>>();

         var repositoryFactory = new Mock<IRepositoryFactory>();
         var defaultModelObjectRepository = new Repository<DefaultModelObject, DefaultEntity>(new MockDataRepository<DefaultEntity>(), modelObjectFactory);
         repositoryFactory.Setup(x => x.CreateRepository<DefaultModelObject>()).Returns(defaultModelObjectRepository);
         var modelObjectViewModelFactory = new ModelObjectViewModelFactory();

         _repositoryEditorViewModel = new RepositoryEditorViewModel<DefaultModelObject>(repositoryFactory.Object, new Domain.ModelObjects.User(), modelObjectViewModelFactory);

         defaultModelObjectRepository.CreateAsync(1).Wait();
         defaultModelObjectRepository.CreateAsync(2).Wait();
         defaultModelObjectRepository.CreateAsync(1).Wait();
         defaultModelObjectRepository.CreateAsync(1).Wait();
      }

      [Fact]
      public async Task EditUserValue_ListIsLockedUntillSaveChanges()
      {
         if (_repositoryEditorViewModel.SearchCommandAsync.CanExecute(null))
         {
            await _repositoryEditorViewModel.SearchCommandAsync.ExecuteAsync(null);
         }

         Assert.Equal(4, _repositoryEditorViewModel.SearchResultViewModels.Count);

         if (_repositoryEditorViewModel.SelectSearchResultCommandAsync.CanExecute(_repositoryEditorViewModel.SearchResultViewModels.First()))
         {
            await _repositoryEditorViewModel.SelectSearchResultCommandAsync.ExecuteAsync(_repositoryEditorViewModel.SearchResultViewModels.First());
         }

         Assert.NotNull(_repositoryEditorViewModel.SelectedSearchResultViewModel);

         _repositoryEditorViewModel.SelectedSearchResultViewModel.ModelObject.CreatedById.Value = 2;

         Assert.True(_repositoryEditorViewModel.IsSelectionLocked);
         Assert.True(_repositoryEditorViewModel.IsAnyDatabaseFieldDirty);
         Assert.False(_repositoryEditorViewModel.SelectSearchResultCommandAsync.CanExecute(_repositoryEditorViewModel.SearchResultViewModels.Last()));

         if (_repositoryEditorViewModel.SaveSelectedSearchResultChangesAsyncCommand.CanExecute(null))
         {
            _repositoryEditorViewModel.SaveSelectedSearchResultChangesAsyncCommand.Execute(null);
         }

         Assert.False(_repositoryEditorViewModel.IsSelectionLocked);
         Assert.False(_repositoryEditorViewModel.IsAnyDatabaseFieldDirty);
         Assert.True(_repositoryEditorViewModel.SelectSearchResultCommandAsync.CanExecute(_repositoryEditorViewModel.SearchResultViewModels.Last()));
      }

      [Fact]
      public async Task EditUserValue_ListIsLockedUntillCancelChanges()
      {
         await _repositoryEditorViewModel.SearchCommandAsync.ExecuteAsync(null);
      }

      private class DefaultModelObject : ModelObject
      {
         
      }

      private class DefaultEntity : EntityBase
      {
         
      }
   }
}
