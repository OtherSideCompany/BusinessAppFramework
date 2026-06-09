using BusinessAppFramework.Application.Actions;
using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Trees;
using BusinessAppFramework.Contracts;
using BusinessAppFramework.Domain.DomainObjects;
using BusinessAppFramework.WebUI.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BusinessAppFramework.WebUI.Components.Editor
{
    public class DomainObjectBranchEditor<TItem> : ComponentBase, IEditable where TItem : DomainObject, new()
    {
        #region Private Fields

        [Inject] protected ILocalizedStringService LocalizedStringService { get; set; } = default!;
        [Inject] protected ITreeGateway TreeGateway { get; set; } = default!;
        [Inject] protected IUserDialogService UserDialogService { get; set; } = default!;
        [Inject] protected IDomainObjectServiceGateway<TItem> DomainObjectServiceGateway { get; set; } = default!;

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
        public List<TItem> Items { get; set; } = new();


        #endregion

        #region Public Methods

        public async Task<List<DomainObjectApplicationActionResultPayload>> SaveChangesAsync()
        {
            var results = new List<DomainObjectApplicationActionResultPayload>();

            foreach (var item in Items)
            {
                results.Add(await DomainObjectServiceGateway.SaveAsync(item));
            }

            return results;
        }

        public async Task CancelChangesAsync()
        {
            Items.Clear();
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

            Items.Clear();

            foreach (var node in Branch.Nodes)
            {
                await AddItemFromNodeAsync(node);
            }

            _loadedParentId = ParentId;
            _loadedBranch = Branch;

            _isLoaded = true;
            _isLoading = false;
        }

        private async Task AddItemFromNodeAsync(Node node)
        {
            var domainObject = await DomainObjectServiceGateway.GetHydratedAsync(node.Id);

            if (domainObject != null)
                Items.Add(domainObject);
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

        protected async Task DeleteItemAsync(TItem? domainObject)
        {
            if (domainObject != null && ParentId != null && Branch != null)
            {
                if (await UserDialogService.ConfirmAsync(LocalizedStringService.Get(MessageKeys.DeleteConfirmationMessage) ?? "delete_msg"))
                {
                    if (await TreeGateway.DeleteNodeAsync(ParentId.Value, domainObject.Id, Branch.ParentChildRelationKey))
                    {
                        Items.Remove(domainObject);
                        Branch?.RemoveNode(domainObject.Id);
                        await OnBranchChanged.InvokeAsync();
                    }
                }
            }
        }

        protected async Task<DataGridEditFormAction> OnCommitedItemChanged(TItem item)
        {
            await ItemChanged.InvokeAsync();
            return DataGridEditFormAction.Close;
        }

        #endregion
    }
}
