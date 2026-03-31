using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAppFramework.Adapter.Requests
{
    public class LoginRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

    }
}
