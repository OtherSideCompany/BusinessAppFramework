using BusinessAppFramework.Adapter.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAppFramework.WebUI.Interfaces
{
    public interface IAuthenticationServiceGateway
    {
        Task<AuthenticationResponse> LoginAsync(string userName, string password);
    }
}
