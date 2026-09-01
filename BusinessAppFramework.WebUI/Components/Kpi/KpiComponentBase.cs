using BusinessAppFramework.Application.Actions;
using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.WebUI.Interfaces;
using Microsoft.AspNetCore.Components;

namespace BusinessAppFramework.WebUI.Components.Kpi
{
    public abstract class KpiComponentBase : ComponentBase
    {
        [Inject] protected ILocalizedStringService LocalizedStringService { get; set; }
        [Inject] protected IApplicationActionExecutionService ApplicationActionExecutionService { get; set; }
        [Parameter] public string KpiKey { get; set; }
        public bool IsLoading { get; set; }

        protected OpenDialogApplicationAction? DrillDownAction { get; set; }

        protected async Task OpenDrillDownAsync()
        {
            if (DrillDownAction == null)
                return;

            await ApplicationActionExecutionService.ExecuteApplicationActionAsync(DrillDownAction);
        }
    }
}
