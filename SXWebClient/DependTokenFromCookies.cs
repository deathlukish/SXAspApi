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
            if (token != null)
            {
                var jsonToken = handler.ReadJwtToken(token);
                var b = jsonToken.Claims;
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiQWRtaW4iLCJleHAiOjE3NjY5MTA5MzYsImlzcyI6Ik15QXV0aFNlcnZlciIsImF1ZCI6Ik15QXV0aENsaWVudCJ9.TLStibMiWWJGz_z5KyGWTvL7X1pOOT9-m8s9cxeQeQU");
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
