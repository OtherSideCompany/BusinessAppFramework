using Microsoft.EntityFrameworkCore;
using Moq;
using OtherSideCore.Domain.ModelObjects;
using OtherSideCore.Domain.Repositories;
using OtherSideCore.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherSideCore.Domain.Tests.Repositories
{
   public class RepositorySearchTests
   {
      private DefaultModelObjectRepository<DefaultModelObject, DefaultEntity> _defaultModelObjectRepository { get; }

      public RepositorySearchTests()
      {
         var defaultEntityRepository = new MockDataRepository<DefaultEntity>();
         _defaultModelObjectRepository = new DefaultModelObjectRepository<DefaultModelObject, DefaultEntity>(defaultEntityRepository, new ModelObjectFactory());

         var defaultModelObject1 = new DefaultModelObject(); defaultModelObject1.RandomProperty.Value = "sapin";
         var defaultModelObject2 = new DefaultModelObject(); defaultModelObject2.RandomProperty.Value = "pastèque";
         var defaultModelObject3 = new DefaultModelObject(); defaultModelObject3.RandomProperty.Value = "truite";
         var defaultModelObject4 = new DefaultModelObject(); defaultModelObject4.RandomProperty.Value = "chaussette";

         _defaultModelObjectRepository.SaveAsync(defaultModelObject1, 1).Wait();
         _defaultModelObjectRepository.SaveAsync(defaultModelObject2, 1).Wait();
         _defaultModelObjectRepository.SaveAsync(defaultModelObject3, 1).Wait();
         _defaultModelObjectRepository.SaveAsync(defaultModelObject4, 1).Wait();
      }

      [Fact]
      public async Task SearchAsync_ResultsAreFetched()
      {
         var repositorySearch = new RepositorySearch<DefaultModelObject>(_defaultModelObjectRepository);
         await repositorySearch.SearchAsync(new List<string>(), new List<Constraint>(), false, CancellationToken.None);

         Assert.Equal(4, repositorySearch.SearchResults.Count);
      }

      [Fact]
      public async Task Dispose_NoSearchResultAreStored()
      {
         var repositorySearch = new RepositorySearch<DefaultModelObject>(_defaultModelObjectRepository);
         await repositorySearch.SearchAsync(new List<string>(), new List<Constraint>(), false, CancellationToken.None);
         repositorySearch.Dispose();

         Assert.Empty(repositorySearch.SearchResults);
      }

      [Fact]
      public async Task AddSearchResult_ResultIsAdded()
      {
         var repositorySearch = new RepositorySearch<DefaultModelObject>(_defaultModelObjectRepository);
         await repositorySearch.SearchAsync(new List<string>(), new List<Constraint>(), false, CancellationToken.None);

         var defaultModelObject = new DefaultModelObject();
         repositorySearch.AddSearchResult(defaultModelObject);

         Assert.Equal(5, repositorySearch.SearchResults.Count);
      }

      [Fact]
      public async Task RemoveSearchResult_ResultIsRemove()
      {
         var repositorySearch = new RepositorySearch<DefaultModelObject>(_defaultModelObjectRepository);
         await repositorySearch.SearchAsync(new List<string>(), new List<Constraint>(), false, CancellationToken.None);

         repositorySearch.RemoveSearchResult((DefaultModelObject)repositorySearch.SearchResults.First());

         Assert.Equal(3, repositorySearch.SearchResults.Count);
      }

      [Fact]
      public async Task GetConstrain_ResultIsRemove()
      {
         var repositorySearch = new RepositorySearch<DefaultModelObject>(_defaultModelObjectRepository);
         await repositorySearch.SearchAsync(new List<string>(), new List<Constraint>(), false, CancellationToken.None);

         repositorySearch.RemoveSearchResult((DefaultModelObject)repositorySearch.SearchResults.First());

         Assert.Equal(3, repositorySearch.SearchResults.Count);
      }
   }
}
