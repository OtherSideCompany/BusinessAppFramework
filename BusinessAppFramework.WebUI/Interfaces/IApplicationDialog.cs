using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAppFramework.WebUI.Interfaces
{
    public interface IApplicationDialog
    {
        public IMudDialogInstance MudDialog { get; set; }
        public string? ExecuteRoute { get; set; }
        public int? DomainObjectId { get; set; }
    }
}
