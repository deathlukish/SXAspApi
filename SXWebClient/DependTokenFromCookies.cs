using Microsoft.AspNetCore.Authentication;
using System.Globalization;
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
            var token = _accessor.HttpContext.Request.Cookies["jwt"];
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
