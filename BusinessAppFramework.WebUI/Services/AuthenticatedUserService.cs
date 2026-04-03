using BusinessAppFramework.WebUI.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace BusinessAppFramework.WebUI.Services
{
    public class AuthenticatedUserService : IAuthenticatedUserService
    {
        #region Fields

        private readonly AuthenticationStateProvider _authStateProvider;

        #endregion

        #region Properties



        #endregion

        #region Constructor

        public AuthenticatedUserService(AuthenticationStateProvider authStateProvider)
        {
            _authStateProvider = authStateProvider;
        }

        #endregion

        #region Public Methods

        public async Task<int> GetAuthenticatedUserIdAsync()
        {
            var state = await _authStateProvider.GetAuthenticationStateAsync();
            var claim = state.User.FindFirst(ClaimTypes.NameIdentifier);
            return int.Parse(claim?.Value ?? "0");
        }

        #endregion

        #region Private Methods



        #endregion

    }
}
