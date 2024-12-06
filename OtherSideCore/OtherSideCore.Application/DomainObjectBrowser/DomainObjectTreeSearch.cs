using Microsoft.Data.SqlClient;
using OtherSideCore.Application.Services;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.DomainObjectBrowser
{
   public abstract class DomainObjectTreeSearch : IDomainObjectTreeSearch
   {
      #region Fields

      protected IDomainObjectQueryServiceFactory _domainObjectQueryServiceFactory;

      protected CancellationTokenSource? _currentSearchCancellationTokenSource;
      private readonly SemaphoreSlim _searchSemaphore = new SemaphoreSlim(1, 1);
      protected CancellationToken _currentSearchCancellationToken => _currentSearchCancellationTokenSource.Token;

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectTreeSearch(IDomainObjectQueryServiceFactory domainObjectQueryServiceFactory)
      {
         _domainObjectQueryServiceFactory = domainObjectQueryServiceFactory;
      }

      #endregion

      #region Public Methods      

      public async Task SearchAsync(DomainObject parent)
      {
         await InitializeSearchAsync();

         try
         {
            await SpecificSearchAsync(parent);
         }
         catch (InvalidOperationException)
         {
            
         }
         catch (SqlException)
         {
            
         }
         catch (OperationCanceledException)
         {
            
         }
         finally
         {
            ShutdownSearch();
         }
      }

      public void Dispose()
      {
         
      }

      #endregion

      #region Private Methods

      protected abstract Task SpecificSearchAsync(DomainObject parent);

      protected async Task InitializeSearchAsync()
      {
         await _searchSemaphore.WaitAsync();
         CreateSearchCancellationTokenSource();
      }

      protected void ShutdownSearch()
      {
         DisposeSearchCancellationTokenSource();
         _searchSemaphore.Release();
      }

      protected void CreateSearchCancellationTokenSource()
      {
         if (_currentSearchCancellationTokenSource != null)
         {
            _currentSearchCancellationTokenSource.Cancel();
            _currentSearchCancellationTokenSource.Dispose();
            _currentSearchCancellationTokenSource = null;
         }

         _currentSearchCancellationTokenSource = new CancellationTokenSource();
         _currentSearchCancellationTokenSource.Token.ThrowIfCancellationRequested();
      }

      protected void DisposeSearchCancellationTokenSource()
      {
         _currentSearchCancellationTokenSource?.Dispose();
         _currentSearchCancellationTokenSource = null;
      }

      #endregion
   }
}
