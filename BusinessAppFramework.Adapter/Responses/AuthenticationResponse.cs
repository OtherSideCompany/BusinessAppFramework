using BusinessAppFramework.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAppFramework.Adapter.Responses
{
    public class AuthenticationResponse
    {
        public bool Success { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = default!;
        public string? Token { get; set; }
        public string? ErrorKey { get; set; } = GlobalVariables.DefaultString;

    }
}
