using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Yacs.Custom.AuthorityHandler.Extensions;
using Yacs.Custom.AuthorityHandler.Models;

namespace Yacs.Custom.AuthorityHandler.Requirements.REST
{
    public class YacsPermissionsHandler : AuthorizationHandler<YacsPermissionsRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly YacsPermissionsHandlerOption _options;

        public YacsPermissionsHandler(
            IHttpContextAccessor httpContextAccessor,
            IHttpClientFactory httpClientFactory,
            YacsPermissionsHandlerOption yacsPermissionsHandlerOption)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
            _options = yacsPermissionsHandlerOption;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            YacsPermissionsRequirement requirement)
        {
            if (!context.User.Identity.IsAuthenticated)
                return;

            if (!string.IsNullOrEmpty(_options.SuperAdminRole) &&
                context.User.IsInRole("super_admin"))
            {
                context.Succeed(requirement);
                return;
            }

            string scopeIdentifier = GetScopeValue();

            var httpClient = _httpClientFactory.CreateClient(YacsPermissionsConstants.RestApiName);
            await AttachAccessToken(httpClient);

            var path = _options.RestPath.Replace("{ScopeIdentifier}", scopeIdentifier).Replace("{UserIdentifier}", context.User.GetUserId());

            var permissions = await httpClient.GetFromJsonAsync<List<string>>(path);
            if (permissions != null && permissions.Any(p => permissions.Contains(p)))
            {
                context.Succeed(requirement);
            }

        }

        private async Task AttachAccessToken(HttpClient httpClient)
        {
            if (_options.IncludeAccessToken)
            {
                var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            }
        }

        private string GetScopeValue()
        {
            //get the scope identifier
            switch (_options.ScopeIdentifierSource)
            {
                case Enums.ScopeIdentifierSource.RouteData:
                    var routeValues = _httpContextAccessor.HttpContext.Request.RouteValues;
                    if (!routeValues.TryGetValue(_options.ScopeIdentifierKey, out var scopeRouteDataValue))
                        return null;

                    return scopeRouteDataValue.ToString();
                case Enums.ScopeIdentifierSource.Header:
                    var headers = _httpContextAccessor.HttpContext.Request.Headers;
                    if (!headers.TryGetValue(_options.ScopeIdentifierKey, out var scopeHeaderValue))
                        return null;
                    return scopeHeaderValue;
            }
            return null;
        }
    }
}
