using System;
using System.Collections.Generic;
using System.Text;

namespace OtherSideCore.WebUI.Services
{
    public record HttpResult<T>
    (
        bool Success,
        T? Data,
        string? ErrorMessage,
        int? StatusCode = null
    );
}
