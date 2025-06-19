using OtherSideCore.Application.Factories;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.Tree
{
   public abstract class DomainObjectTree : IDomainObjectTree
   {
      #region Fields

      protected IDomainObjectServiceFactory _domainObjectServiceFactory;

      protected CancellationTokenSource? _currentSearchCancellationTokenSource;
      private readonly SemaphoreSlim _searchSemaphore = new SemaphoreSlim(1, 1);
      protected CancellationToken _currentSearchCancellationToken => _currentSearchCancellationTokenSource.Token;

      #endregion

      #region Properties

      public Func<DomainObject, Task>? FillDomainObjectTreeAsync { get; set; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectTree(IDomainObjectServiceFactory domainObjectServiceFactory)
      {
         _domainObjectServiceFactory = domainObjectServiceFactory;
      }

      #endregion

      #region Public Methods      

      public async Task FillDomainObjectAsync(DomainObject parent)
      {
         await InitializeAsync();

         try
         {
            if (FillDomainObjectTreeAsync != null)
            {
               await FillDomainObjectTreeAsync(parent);
            }
            else
            {
               throw new InvalidOperationException("GetTreeAsync is not set. Please set it before calling GetAsync.");
            }
         }
         catch (OperationCanceledException)
         {

         }
         finally
         {
            Shutdown();
         }
      }

      public void Dispose()
      {

      }

      #endregion

      #region Private Methods

      protected async Task InitializeAsync()
      {
         await _searchSemaphore.WaitAsync();
         CreateCancellationTokenSource();
      }

      protected void Shutdown()
      {
         DisposeSearchCancellationTokenSource();

         if (_searchSemaphore.CurrentCount == 0)
         {
            _searchSemaphore.Release();
         }
      }

      protected void CreateCancellationTokenSource()
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
