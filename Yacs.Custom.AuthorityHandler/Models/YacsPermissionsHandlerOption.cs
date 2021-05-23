using System;
using System.Collections.Generic;
using System.Text;
using Yacs.Custom.AuthorityHandler.Enums;

namespace Yacs.Custom.AuthorityHandler.Models
{
    public class YacsPermissionsHandlerOption
    {

        public string ScopeIdentifierKey { get; set; }

        public ScopeIdentifierSource ScopeIdentifierSource { get; set; } = ScopeIdentifierSource.RouteData;

        /// <summary>
        /// Only used when source = REST
        /// </summary>
        public Uri RestSource { get; set; } = null;

        /// <summary>
        /// Path to get list of permissions, ex: "/api/permissions/{ScopeIdentifier}/user/{UserIdentifier}"
        /// </summary>
        public string RestPath { get; set; }

        /// <summary>
        /// Role that allows user to do everything! ex: super_admin, set null to remove feature
        /// </summary>
        public string SuperAdminRole { get; set; }

        /// <summary>
        /// pull access token from current context
        /// </summary>
        public bool IncludeAccessToken { get; set; }
    }
}
