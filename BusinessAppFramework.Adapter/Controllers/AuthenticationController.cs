using BusinessAppFramework.Adapter.Requests;
using BusinessAppFramework.Adapter.Responses;
using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Contracts.ApiRoutes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAppFramework.Adapter.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route($"{ApiRouteSegments.Root}/{ApiRouteSegments.Authentication}")]
    public class AuthenticationController : ControllerBase
    {
        #region Fields

        private readonly IAuthenticationService _authenticationService;
        private readonly IAuthenticationTokenService _authenticationTokenService;

        #endregion

        #region Constructor

        public AuthenticationController(
            IAuthenticationService authenticationService,
            IAuthenticationTokenService authenticationTokenService)
        {
            _authenticationService = authenticationService;
            _authenticationTokenService = authenticationTokenService;
        }

        #endregion

        #region Public Methods

        [HttpPost(ApiRouteSegments.Login)]
        public async Task<ActionResult<AuthenticationResponse>> Login([FromBody] LoginRequest request)
        {
            var (succes, userId) = await _authenticationService.VerifyPasswordAsync(request.UserName, request.Password);
            var authenticationResponse = new AuthenticationResponse();

            if (succes)
            {
                authenticationResponse.Success = true;
                authenticationResponse.Token = _authenticationTokenService.GenerateAccessToken(userId);
                authenticationResponse.UserId = userId;
            }
            else
            {
                authenticationResponse.ErrorMessage = "Invalid username or password.";
            }

            return Ok(authenticationResponse);
        }
        #endregion
    }
}
