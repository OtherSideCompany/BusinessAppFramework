using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAppFramework.WebUI.Authentication
{
    public class SignInRequest
    {
        public int UserId { get; set; }
        public string Token { get; set; } = default!;
    }
}
