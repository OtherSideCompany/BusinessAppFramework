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

        protected abstract string PageTreeKey { get; }

        protected bool _isDeleted;
        protected Tree? _tree;
        protected int? _loadedId;
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

        public async Task UpdateTreeBranchAsync(string pageTreeKey, string relationKey)
        {
            if (Id.HasValue)
            {
                var branch = await TreeGateway.GetTreeBranchAsync(Id.Value, pageTreeKey, relationKey);

                if (branch == null)
                    return;

                _tree?.SetBranch(branch);
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

        public async Task ExecuteApplicationActionAsync(IApplicationAction applicationAction)
        {
            if (applicationAction is IDomainObjectApplicationAction domainObjectApplicationAction && Id != null)
            {
                domainObjectApplicationAction.DomainObjectId = Id.Value;
            }

            var payload = await ApplicationActionExecutionService.ExecuteApplicationActionAsync(applicationAction);

            if (payload?.ErrorMessageKey != null)
            {
                UserDialogService.SnackError(LocalizedStringService.Get(payload?.ErrorMessageKey!));
            }
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
            StateHasChanged();
        }

        protected async Task SaveAsync()
        {
            if (DomainObject != null)
            {
                await DomainObjectServiceGateway.SaveAsync(DomainObject);

                foreach (var editableComponent in _editableComponents)
                {
                    await editableComponent.SaveChangesAsync();
                }

                SetDirtyState(false);

                await LoadPageAsync(true);
            }
        }

        protected async Task UndoAsync()
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

        private void SetDirtyState(bool isDirty)
        {
            IsDirty = isDirty;
            StateHasChanged();
        }

        #endregion
    }
}
