using System.Diagnostics;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using OneStream.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OneStream.Backend.Components.Account
{
    public class PersistingAuthenticationStateProvider : ServerAuthenticationStateProvider, IDisposable
    {
        private Task<AuthenticationState>? _authenticationStateTask;
        private readonly PersistentComponentState _state;
        private readonly PersistingComponentStateSubscription _subscription;
        private readonly IdentityOptions _options;
        private readonly IConfiguration _configuration;

        public PersistingAuthenticationStateProvider(
            PersistentComponentState persistentComponentState,
            IOptions<IdentityOptions> optionsAccessor,
            IConfiguration configuration)
        {
            _options = optionsAccessor.Value;
            _state = persistentComponentState;
            AuthenticationStateChanged += OnAuthenticationStateChanged;
            _subscription = _state.RegisterOnPersisting(OnPersistingAsync, RenderMode.InteractiveWebAssembly);
            _configuration = configuration;
        }

        private async Task OnPersistingAsync()
        {
            if (_authenticationStateTask is null)
                throw new UnreachableException($"Authentication state not set in {nameof(OnPersistingAsync)}().");

            var authenticationState = await _authenticationStateTask;
            var principal = authenticationState.User;

            if (principal.Identity?.IsAuthenticated == true)
            {
                var userId = principal.FindFirst(_options.ClaimsIdentity.UserIdClaimType)?.Value;
                var email = principal.FindFirst(_options.ClaimsIdentity.EmailClaimType)?.Value;

                var jwtToken = new JwtSecurityToken(
                    claims: new List<Claim> {
                    new(ClaimTypes.NameIdentifier, userId),
                    new(ClaimTypes.Name, email),
                    },
                    notBefore: DateTime.UtcNow,
                    expires: DateTime.UtcNow.AddMinutes(30),
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(_configuration["Authentication:JWT_Secret"])
                        ),
                        SecurityAlgorithms.HmacSha256Signature)
                );

                _state.PersistAsJson(nameof(UserInfo), new UserInfo
                {
                    UserId = userId,
                    Email = email,
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtToken)
                });
            }
        }

        private void OnAuthenticationStateChanged(Task<AuthenticationState> authenticationStateTask) => _authenticationStateTask = authenticationStateTask;

        public void Dispose()
        {
            _authenticationStateTask?.Dispose();
            AuthenticationStateChanged -= OnAuthenticationStateChanged;
            _subscription.Dispose();
        }
    }
}
