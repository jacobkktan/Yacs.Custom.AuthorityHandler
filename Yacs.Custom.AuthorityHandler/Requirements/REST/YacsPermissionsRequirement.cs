using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yacs.Custom.AuthorityHandler.Requirements.REST
{
    public class YacsPermissionsRequirement : IAuthorizationRequirement
    {

        public YacsPermissionsRequirement(
            params string[] allowedPermissions
            )
        {
            AllowedPermissions = allowedPermissions;
        }

        public string[] AllowedPermissions { get; }
    }
}
