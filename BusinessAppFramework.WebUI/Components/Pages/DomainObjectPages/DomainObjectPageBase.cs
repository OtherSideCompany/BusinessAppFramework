using BusinessAppFramework.Application.Actions;
using BusinessAppFramework.Application.Descriptors;
using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Search;
using BusinessAppFramework.Application.Trees;
using BusinessAppFramework.Contracts;
using BusinessAppFramework.Domain.DomainObjects;
using BusinessAppFramework.WebUI.Components.Editor;
using BusinessAppFramework.WebUI.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using MudBlazor;
using Search.Contracts.SearchResults;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BusinessAppFramework.WebUI.Components.Pages.DomainObjectPages
{
    public abstract class DomainObjectPageBase<TDomainObject, TSearchResult> : ComponentBase, IDisposable 
        where TDomainObject : DomainObject, new()
        where TSearchResult : DomainObjectSearchResult, new()
    {
        #region Fields

        [Inject] protected ILocalizedStringService LocalizedStringService { get; set; } = default!;
        [Inject] protected IDomainObjectServiceGateway<TDomainObject> DomainObjectServiceGateway { get; set; } = default!;
        [Inject] protected ITreeGateway TreeGateway { get; set; } = default!;
        [Inject] protected IUserDialogService UserDialogService { get; set; } = default!;
        [Inject] protected IApplicationActionExecutionService ApplicationActionExecutionService { get; set; } = default!;
        [Inject] protected IDialogService DialogService { get; set; } = default!;
        [Inject] protected IIconFactory IconFactory { get; set; } = default!;
        [Inject] protected IRelationServiceGateway RelationServiceGateway { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] protected ISearchGateway<TSearchResult> SearchGateway { get; set; } = default!;
        [Inject] protected IDocumentServiceGateway DocumentServiceGateway { get; set; } = default!;

        protected virtual string? PageTreeKey { get; }
        protected virtual string? DocumentsRelationKey => null;

        protected bool _isDeleted;
        protected Tree? _tree;
        protected int? _loadedId;
        private int _documentsCount;
        protected Workflow.Workflow? _workflow;
        private List<IEditable> _editableComponents { get; set; } = new();

        private IDisposable? _locationChangingRegistration;

        protected TSearchResult? _searchResult;

        #endregion

        #region Properties

        [Parameter] public WorkspaceDescriptor Descriptor { get; set; } = default!;
        [Parameter, SupplyParameterFromQuery] public int? Id { get; set; }
        public TDomainObject? DomainObject { get; protected set; }
        public bool IsDirty { get; private set; }

        #endregion

        #region Public Methods

        public void RegisterEditableComponent(IEditable? editableComponent)
        {
            if (editableComponent == null || _editableComponents.Contains(editableComponent))
                return;

            _editableComponents.Add(editableComponent);
        }

        public Branch? GetTreeBranch(string relationKey)
        {
            return _tree?.GetBranch(relationKey);
        }

        public async Task UpdateTreeBranchAsync(string relationKey)
        {
            if (Id.HasValue && PageTreeKey != null)
            {
                var branch = await TreeGateway.GetTreeBranchAsync(Id.Value, PageTreeKey, relationKey);

                if (branch == null)
                    return;

                _tree?.SetBranch(branch);

                StateHasChanged();
            }
        }

        public void MarkDirty()
        {
            if (!IsDirty)
            {
                SetDirtyState(true);
            }
        }

        public async Task DeleteAsync()
        {
            if (Id == null)
                return;

            if (await UserDialogService.ConfirmAsync(LocalizedStringService.Get(MessageKeys.DeleteConfirmationMessage) ?? "delete_msg"))
            {
                var payload = await DomainObjectServiceGateway.DeleteAsync(Id.Value);

                DisplayPayloadMessage(payload);

                _isDeleted = payload.Changes.Any(c => c.ChangeType == ChangeType.Deleted);

                StateHasChanged();
            }
        }

        public async Task DeleteTreeNodeAsync(int parentId, int childId, string relationKey)
        {
            if (await UserDialogService.ConfirmAsync(LocalizedStringService.Get(BusinessAppFramework.Contracts.MessageKeys.DeleteConfirmationMessage) ?? "delete_msg"))
            {
                var payload = await TreeGateway.DeleteNodeAsync(parentId, childId, relationKey);

                DisplayPayloadMessage(payload);

                await UpdateTreeBranchAsync(relationKey);

                StateHasChanged();
            }
        }

        public virtual async Task ExecuteApplicationActionAsync(IApplicationAction applicationAction, int? id)
        {
            if (id != null)
            {
                if (applicationAction is IDomainObjectApplicationAction domainObjectApplicationAction)
                {
                    domainObjectApplicationAction.DomainObjectId = id.Value;
                }
                else if (applicationAction is IOpenDialogApplicationAction openDialogApplicationAction)
                {
                    openDialogApplicationAction.DomainObjectId = id.Value;
                }
            }

            var payload = await ApplicationActionExecutionService.ExecuteApplicationActionAsync(applicationAction);

            DisplayPayloadMessage(payload);
        }

        public virtual async Task ExecuteApplicationActionAsync(IApplicationAction applicationAction)
        {
            await ExecuteApplicationActionAsync(applicationAction, Id);
        }

        public void Dispose()
        {
            _locationChangingRegistration?.Dispose();
        }

        #endregion

        #region Private Methods 

        protected override void OnInitialized()
        {
            _locationChangingRegistration = NavigationManager.RegisterLocationChangingHandler(HandleLocationChanging);
        }

        private async ValueTask HandleLocationChanging(LocationChangingContext context)
        {
            if (!IsDirty)
                return;

            if (!await UserDialogService.ConfirmAsync(LocalizedStringService.Get(MessageKeys.UnsavedChangedDetectedProceed)))
                context.PreventNavigation();
        }

        protected void DisplayPayloadMessage(DomainObjectApplicationActionResultPayload? payload)
        {
            if (payload == null)
                return;

            if (payload?.ErrorMessageKey != null)
            {
                UserDialogService.SnackError(LocalizedStringService.Get(payload.ErrorMessageKey));
            }

            if (payload?.ConfirmationMessageKey != null)
            {
                UserDialogService.SnackShow(LocalizedStringService.Get(payload.ConfirmationMessageKey));
            }
        }

        protected virtual void UpdatePropertiesEditorAvailableValuesParentIds()
        {
            
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            await LoadPageAsync();
        }

        protected virtual async Task LoadPageAsync(bool force = false)
        {
            if (!Id.HasValue)
                return;

            if (!force && _loadedId == Id.Value)
                return;

            DomainObject = await DomainObjectServiceGateway.GetHydratedAsync(Id.Value);
            _searchResult = await SearchGateway.GetSearchResultAsync(Id.Value);

            if (!string.IsNullOrEmpty(PageTreeKey))
            {
                _tree = await TreeGateway.GetTreeAsync(Id.Value, PageTreeKey);
            }

            if (!string.IsNullOrEmpty(DocumentsRelationKey))
            {
                _documentsCount = await DocumentServiceGateway.GetDocumentsCountAsync(Id.Value, DocumentsRelationKey);
            }

            _loadedId = Id.Value;

            if (_workflow != null)
            {
                await _workflow.RefreshWorkflowAsync();
            }

            UpdatePropertiesEditorAvailableValuesParentIds();

            StateHasChanged();
        }

        protected virtual async Task SaveAsync()
        {
            var results = new List<DomainObjectApplicationActionResultPayload>();

            if (DomainObject != null)
            {
                results.Add(await DomainObjectServiceGateway.SaveAsync(DomainObject));

                foreach (var editableComponent in _editableComponents)
                {
                    results.AddRange(await editableComponent.SaveChangesAsync());
                }

                foreach (var result in results.Where(r => r.ErrorMessageKey != null))
                {
                    UserDialogService.SnackError(LocalizedStringService.Get(result.ErrorMessageKey!));
                }

                SetDirtyState(false);

                await LoadPageAsync(true);
            }
        }

        protected virtual async Task UndoAsync()
        {
            if (DomainObject != null)
            {
                DomainObject = await DomainObjectServiceGateway.GetHydratedAsync(DomainObject.Id);

                foreach (var editableComponent in _editableComponents)
                {
                    await editableComponent.CancelChangesAsync();
                }

                SetDirtyState(false);

                await LoadPageAsync(true);
            }
        }

        protected int? CountBranchItemsForTabBadge(string relationKey)
        {
            var branch = GetTreeBranch(relationKey);
            return branch?.Nodes.Count != 0 ? branch?.Nodes.Count : null;
        }

        protected int? DocumentsCountForTabBadge => _documentsCount > 0 ? _documentsCount : null;

        protected void OnDocumentsCountChanged(int count)
        {
            _documentsCount = count;
            StateHasChanged();
        }

        private void SetDirtyState(bool isDirty)
        {
            IsDirty = isDirty;
            StateHasChanged();
        }

        protected string GetDomainObjectBrowserWorkspaceLink(string workspaceKey, string? filter)
        {
            var href = "/workspace/" + workspaceKey;

            if (!string.IsNullOrEmpty(filter))
            {
                href += $"?search={filter}";
            }

            return href;
        }

        protected async Task<DomainObjectReference> ResolveReferenceAsync(
            DomainObjectReference current,
            int accountId,
            Func<int, CancellationToken, Task<int?>> getId)
        {
            var id = await getId(accountId, CancellationToken.None);

            if (!id.HasValue)
                return new DomainObjectReference { RelationKey = current.RelationKey };

            var hydrated = await RelationServiceGateway.GetHydratedReferenceAsync(
                accountId, id.Value, current.RelationKey);

            return new DomainObjectReference
            {
                RelationKey = current.RelationKey,
                DomainObjectId = hydrated?.DomainObjectId,
                DisplayValue = hydrated?.DisplayValue
            };
        }

        protected virtual List<PropertyCategory> GetPropertyCategories()
        {
            return new List<PropertyCategory>();
        }

        #endregion
    }
}
