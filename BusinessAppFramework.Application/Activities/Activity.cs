using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Search;

namespace BusinessAppFramework.Application.Activities
{
    public class Activity<TSearchResult> : IActivity where TSearchResult : DomainObjectSearchResult, new()
    {
        #region Fields

        private ISearchGateway<TSearchResult> _searchGateway;

        #endregion

        #region Properties

        public string ActivityKey { get; set; }
        public string TargetWorkspaceKey { get; set; }
        public string TargetWorkspaceConstraintKey { get; set; }
        public int Count { get; set; }

        #endregion

        #region Events



        #endregion

        #region Constructor

        public Activity(ISearchGateway<TSearchResult> searchGateway)
        {
            _searchGateway = searchGateway;
        }

        #endregion

        #region Public Methods

        public async Task ComputeCountAsync()
        {
            Count = await _searchGateway.CountAsync(new SearchRequest() { ConstraintKey = TargetWorkspaceConstraintKey });
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
