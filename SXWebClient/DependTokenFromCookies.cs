using Microsoft.AspNetCore.Authentication;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;

namespace SXWebClient
{
    public sealed class DependTokenFromCookies : DelegatingHandler
    {
        private readonly IHttpContextAccessor _accessor;

        public DependTokenFromCookies(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = _accessor.HttpContext.Request.Cookies["jwt"];
            if (!string.IsNullOrEmpty(token))
            {
                var jsonToken = handler.ReadJwtToken(token);
                var b = jsonToken.Claims;
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
