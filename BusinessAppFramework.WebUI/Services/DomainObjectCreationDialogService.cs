using BusinessAppFramework.Application.Actions;
using BusinessAppFramework.Application.Descriptors;
using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Contracts;
using BusinessAppFramework.WebUI.Interfaces;

namespace BusinessAppFramework.WebUI.Services
{
    public class DomainObjectCreationDialogService : IDomainObjectCreationDialogService
    {
        #region Fields

        private readonly IApplicationActionExecutionService _applicationActionExecutionService;
        private readonly ILocalizedStringService _localizedStringService;
        private readonly IUserDialogService _userDialogService;

        #endregion

        #region Constructor

        public DomainObjectCreationDialogService(
            IApplicationActionExecutionService applicationActionExecutionService,
            ILocalizedStringService localizedStringService,
            IUserDialogService userDialogService)
        {
            _applicationActionExecutionService = applicationActionExecutionService;
            _localizedStringService = localizedStringService;
            _userDialogService = userDialogService;
        }

        #endregion

        #region Public Methods

        public async Task<int?> CreateAsync(IDomainObjectSelectorDescriptor descriptor)
        {
            if (string.IsNullOrEmpty(descriptor.CreationDialogComponentKey))
                return null;

            var openDialogApplicationAction = new OpenDialogApplicationAction
            {
                ActionKey = StringKeys.CreationKey,
                ComponentKey = descriptor.CreationDialogComponentKey,
                DialogTitle = _localizedStringService.Get(descriptor.CreationDialogComponentKey)
            };

            var payload = await _applicationActionExecutionService.ExecuteApplicationActionAsync(openDialogApplicationAction);

            if (payload == null)
                return null;

            if (payload.ErrorMessageKey != null)
            {
                _userDialogService.SnackError(_localizedStringService.Get(payload.ErrorMessageKey));
                return null;
            }

            return payload.Changes.FirstOrDefault(c => c.ChangeType == ChangeType.Added)?.DomainObjectId;
        }

        #endregion
    }
}
