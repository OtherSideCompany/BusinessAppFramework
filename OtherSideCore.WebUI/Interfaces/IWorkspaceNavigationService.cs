using System;
using System.Collections.Generic;
using System.Text;

namespace OtherSideCore.WebUI.Interfaces
{
    public interface IWorkspaceNavigationService
    {
        Task<List<string>> GetModuleKeysAsync();
        Task<List<string>> GetWorkspaceKeysAsync(string moduleKey);
    }
}
