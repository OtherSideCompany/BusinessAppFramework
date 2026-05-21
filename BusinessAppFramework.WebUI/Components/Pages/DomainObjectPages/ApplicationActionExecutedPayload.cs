using BusinessAppFramework.Application.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAppFramework.WebUI.Components.Pages.DomainObjectPages
{
    public class ApplicationActionExecutedPayload
    {
        public string ActionKey { get; init; } = default!;
        public DomainObjectApplicationActionResultPayload? ActionResult { get; init; }
    }
}
