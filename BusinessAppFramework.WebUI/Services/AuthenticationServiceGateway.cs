using BusinessAppFramework.Adapter.Requests;
using BusinessAppFramework.Adapter.Responses;
using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Contracts.ApiRoutes;
using BusinessAppFramework.WebUI.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAppFramework.WebUI.Services
{
    public class AuthenticationServiceGateway : HttpService, IAuthenticationServiceGateway
    {
        #region Fields

        private string _baseUrl => $"{ApiRouteSegments.Root}/{ApiRouteSegments.Authentication}";

        #endregion

        #region Properties



        #endregion

        #region Constructor

        public AuthenticationServiceGateway(
            IHttpClientFactory clientFactory, 
            IOptions<ApiClientOptions> apiClientOptions, 
            ILogger<HttpService> logger, 
            ILocalizedStringService localizedStringService, 
            IUserDialogService userDialogService) : 
            base(clientFactory, apiClientOptions, logger, localizedStringService, userDialogService)
        {

        }

        #endregion

        #region Public Methods

        public async Task<AuthenticationResponse> LoginAsync(string userName, string password)
        {
            var route = $"{_baseUrl}/{ApiRouteSegments.Login}";

            var request = new LoginRequest
            {
                UserName = userName,
                Password = password
            };

            var result = await PostAsync<AuthenticationResponse>(route, request);

            return result.Data ?? new AuthenticationResponse { Success = false, ErrorKey = Contracts.MessageKeys.ServerError };
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
