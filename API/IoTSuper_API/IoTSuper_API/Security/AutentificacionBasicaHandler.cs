using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Encodings.Web;

namespace IoTSuper_API.Security
{
    public class AutentificacionBasicaHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly AutenticacionBasica _autenticacionBasica;
        public AutentificacionBasicaHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, IOptions<AutenticacionBasica> basicAuthOptions) : base(options, logger, encoder)
        {
            _autenticacionBasica = basicAuthOptions.Value;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return Task.FromResult(AuthenticateResult.Fail("Error"));
            }

            try
            {
                if (!AuthenticationHeaderValue.TryParse(Request.Headers["Authorization"], out AuthenticationHeaderValue authHeader))
                {
                    return Task.FromResult(AuthenticateResult.Fail("Error"));
                }

                byte[] credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                List<string> credentials = new List<string>(System.Text.Encoding.UTF8.GetString(credentialBytes).Split(':', 2));

                string username = credentials[0];
                string password = credentials[1];

                if (username == _autenticacionBasica.AutentificacionBasicaUsuario && password == _autenticacionBasica.AutentificacionBasicaContrasena)
                {
                    var claims = new[] { new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, username) };
                    var identity = new System.Security.Claims.ClaimsIdentity(claims, Scheme.Name);
                    var principal = new System.Security.Claims.ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);
                    return Task.FromResult(AuthenticateResult.Success(ticket));
                }
                else
                {
                    return Task.FromResult(AuthenticateResult.Fail("Error"));
                }

            }
            catch
            {
                return Task.FromResult(AuthenticateResult.Fail("Error"));
            }
        }
    }
}
