using MudBlazor;
using OtherSideCore.Application.Search;
using System;
using System.Collections.Generic;
using System.Text;

namespace OtherSideCore.WebUI.Components.Selector
{
    public interface IDomainObjectSelector
    {
        Task Load(string filter);
    }
}
