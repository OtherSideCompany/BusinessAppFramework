using BusinessAppFramework.Application.Actions;
using BusinessAppFramework.Application.Descriptors;
using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Trees;
using BusinessAppFramework.Contracts;
using BusinessAppFramework.Domain.DomainObjects;
using BusinessAppFramework.WebUI.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BusinessAppFramework.WebUI.Components.Pages.DomainObjectPages
{
    public abstract class DomainObjectPageBase<TDomainObject> : ComponentBase where TDomainObject : DomainObject, new()
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

        protected virtual string? PageTreeKey { get; }

        protected bool _isDeleted;
        protected Tree? _tree;
        protected int? _loadedId;
        protected Workflow.Workflow? _workflow;
        private List<IEditable> _editableComponents { get; set; } = new();

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
                await DomainObjectServiceGateway.DeleteAsync(Id.Value);
                _isDeleted = true;

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

            if (payload?.ErrorMessageKey != null)
            {
                UserDialogService.SnackError(LocalizedStringService.Get(payload?.ErrorMessageKey!));
            }
        }

        public virtual async Task ExecuteApplicationActionAsync(IApplicationAction applicationAction)
        {
            await ExecuteApplicationActionAsync(applicationAction, Id);
        }

        #endregion

        #region Private Methods        

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

            if (!string.IsNullOrEmpty(PageTreeKey))
            {
                _tree = await TreeGateway.GetTreeAsync(Id.Value, PageTreeKey);
            }

            _loadedId = Id.Value;

            if (_workflow != null)
            {
                await _workflow.RefreshWorkflowAsync();
            }

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

        private void SetDirtyState(bool isDirty)
        {
            IsDirty = isDirty;
            StateHasChanged();
        }

        #endregion
    }
}
