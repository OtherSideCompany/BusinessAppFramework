using BusinessAppFramework.Application.Actions;
using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Search;
using BusinessAppFramework.Application.Trees;
using BusinessAppFramework.Contracts;
using BusinessAppFramework.Domain.DomainObjects;
using BusinessAppFramework.WebUI.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BusinessAppFramework.WebUI.Components.Editor
{
    public class DomainObjectBranchEditor<TDomainObject, TSearchResult> : ComponentBase, IEditable 
        where TDomainObject : DomainObject, new()
        where TSearchResult : DomainObjectSearchResult, new()
    {
        #region Private Fields

        [Inject] protected ILocalizedStringService LocalizedStringService { get; set; } = default!;
        [Inject] protected ITreeGateway TreeGateway { get; set; } = default!;
        [Inject] protected IUserDialogService UserDialogService { get; set; } = default!;
        [Inject] protected IDomainObjectServiceGateway<TDomainObject> DomainObjectServiceGateway { get; set; } = default!;
        [Inject] protected ISearchGateway<TSearchResult> DomainObjectSearchGateway { get; set; } = default!;

        protected bool _isLoaded;
        protected bool _isLoading;
        protected int? _loadedParentId;
        protected Branch? _loadedBranch;

        protected Task NotifyItemChanged() => ItemChanged.InvokeAsync();

        #endregion

        #region Properties

        [Parameter, EditorRequired] public int? ParentId { get; set; }
        [Parameter, EditorRequired] public Branch? Branch { get; set; }
        [Parameter] public EventCallback OnBranchChanged { get; set; }
        [Parameter] public EventCallback ItemChanged { get; set; } 
        public IEnumerable<Node> VisibleNodes => CollectVisible(Branch?.Nodes ?? Enumerable.Empty<Node>());

        #endregion

        #region Constructor

        public DomainObjectBranchEditor()
        {
            
        }

        #endregion

        #region Public Methods

        public async Task<List<DomainObjectApplicationActionResultPayload>> SaveChangesAsync()
        {
            var results = new List<DomainObjectApplicationActionResultPayload>();

            foreach (var node in FlattenNodes(Branch?.Nodes))
            {
                if (node.DomainObject is TDomainObject item)
                {
                    results.Add(await DomainObjectServiceGateway.SaveAsync(item));
                }
            }          
            
            return results;
        }

        public async Task CancelChangesAsync()
        {
            await LoadAsync(true);
        }

        #endregion

        #region private Methods    

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            if (ParentId != _loadedParentId || Branch != _loadedBranch)
            {
                await LoadAsync(true);
            }
            else
            {
                await LoadAsync(false);
            }
        }

        protected async Task LoadAsync(bool refresh)
        {
            if ((_isLoaded && !refresh) || _isLoading || Branch is null)
                return;

            _isLoading = true;

            var domainObjects = await DomainObjectServiceGateway.GetAllHydratedAsync(FlattenNodes(Branch?.Nodes).Select(n => n.Id).ToList());
            var searchResults = await DomainObjectSearchGateway.GetSearchResultsAsync(FlattenNodes(Branch?.Nodes).Select(n => n.Id).ToList());

            foreach (var node in FlattenNodes(Branch?.Nodes))
            {
                node.DomainObject = domainObjects.First(d => d.Id == node.Id);
                node.DomainObjectSearchResult = searchResults.First(s => s.DomainObjectId == node.Id);
            }

            _loadedParentId = ParentId;
            _loadedBranch = Branch;

            _isLoaded = true;
            _isLoading = false;
        }

        private async Task AddItemFromNodeAsync(Node node)
        {
            var domainObject = await DomainObjectServiceGateway.GetHydratedAsync(node.Id);
            node.DomainObject = domainObject;
        }

        protected async virtual Task CreateItemAsync()
        {
            if (ParentId == null || Branch == null)
            {
                return;
            }

            var node = await TreeGateway.CreateNode(ParentId.Value, Branch.ParentChildRelationKey);

            if (node != null)
            {
                Branch.AddNode(node);
                await AddItemFromNodeAsync(node);
                await OnBranchChanged.InvokeAsync();
            }
        }

        protected async Task DeleteItemAsync(int id)
        {         
            if (ParentId != null && Branch != null)
            {
                if (await UserDialogService.ConfirmAsync(LocalizedStringService.Get(MessageKeys.DeleteConfirmationMessage) ?? "delete_msg"))
                {
                    if (await TreeGateway.DeleteNodeAsync(ParentId.Value, id, Branch.ParentChildRelationKey))
                    {
                        RemoveNodeRecursive(new List<Branch> { Branch }, id, Branch.ParentChildRelationKey);
                        Branch?.RemoveNode(id);
                        await OnBranchChanged.InvokeAsync();
                    }
                }
            }
        }

        protected async Task<DataGridEditFormAction> OnCommitedItemChanged(Node node)
        {
            await ItemChanged.InvokeAsync();
            return DataGridEditFormAction.Close;
        }

        private static IEnumerable<Node> FlattenNodes(IEnumerable<Node>? nodes)
        {
            if (nodes == null)
                yield break;

            foreach (var node in nodes)
            {
                yield return node;

                foreach (var childBranch in node.ChildBranches)
                {
                    foreach (var descendant in FlattenNodes(childBranch.Nodes))
                    {
                        yield return descendant;
                    }
                }
            }
        }

        private IEnumerable<Node> CollectVisible(IEnumerable<Node> nodes)
        {
            foreach (var node in nodes)
            {
                yield return node;

                if (node.IsExpanded)
                {
                    foreach (var childBranch in node.ChildBranches)
                    {
                        foreach (var descendant in CollectVisible(childBranch.Nodes))
                            yield return descendant;
                    }
                }
            }
        }

        private static bool RemoveNodeRecursive(IEnumerable<Branch> branches, int id, string relationKey)
        {
            foreach (var branch in branches)
            {
                if (branch.ParentChildRelationKey == relationKey && branch.Nodes.Any(n => n.Id == id))
                {
                    branch.RemoveNode(id);
                    return true;
                }

                foreach (var node in branch.Nodes)
                {
                    if (RemoveNodeRecursive(node.ChildBranches, id, relationKey))
                        return true;
                }
            }

            return false;
        }

        #endregion
    }
}
