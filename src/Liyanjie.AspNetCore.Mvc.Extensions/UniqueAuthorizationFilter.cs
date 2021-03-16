using System;
using System.Linq;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Extensions
{
    public class UniqueAuthorizationFilter : IAuthorizationFilter
    {
        public const string ClaimType_UniqueId = "UniqueId";

        readonly Func<AuthorizationFilterContext, string> getUserUniqueId;
        public UniqueAuthorizationFilter(Func<AuthorizationFilterContext, string> getUserUniqueId)
        {
            this.getUserUniqueId = getUserUniqueId ?? throw new ArgumentNullException(nameof(getUserUniqueId));
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (true
                && context.ActionDescriptor.FilterDescriptors.Any(_ => _ is IAuthorizeData)
                && !context.ActionDescriptor.FilterDescriptors.Any(_ => _ is IAllowAnonymous)
                && context.HttpContext.User.Identity.IsAuthenticated)
            {
                var tokenUniqueId = context.HttpContext.User.Claims
                    .SingleOrDefault(_ => _.Type == ClaimType_UniqueId)?.Value ?? Guid.NewGuid().ToString("N");
                if (tokenUniqueId == null)
                    goto Unauthorized;

                var userUniqueId = getUserUniqueId.Invoke(context);
                if (tokenUniqueId != userUniqueId)
                    goto Unauthorized;

                Unauthorized:
                context.Result = new StatusCodeResult(401);
            }
        }
    }
}
