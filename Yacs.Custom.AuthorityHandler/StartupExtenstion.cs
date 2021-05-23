using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;
using Yacs.Custom.AuthorityHandler.Models;
using Yacs.Custom.AuthorityHandler.Requirements.REST;

namespace Yacs.Custom.AuthorityHandler
{
    public static class StartupExtenstion
    { 
        public static IServiceCollection AddCustomAuthHandler(this IServiceCollection services, YacsPermissionsHandlerOption option)
        {
            services.AddHttpClient(YacsPermissionsConstants.RestApiName, conf => conf.BaseAddress = option.RestSource);
            services.AddScoped<IAuthorizationHandler, YacsPermissionsHandler>();            
            services.AddSingleton(p => option);
            return services;
        }
    }
}
