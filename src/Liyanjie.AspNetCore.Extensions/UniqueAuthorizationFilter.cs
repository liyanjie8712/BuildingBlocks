using System;
using System.Linq;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Liyanjie.AspNetCore.Extensions
{
    public class UniqueAuthorizationFilter : IAuthorizationFilter
    {
        public const string ClaimType_UniqueIdentity = "UId";

        readonly Func<AuthorizationFilterContext, string> getUserUniqueIdentity;
        public UniqueAuthorizationFilter(Func<AuthorizationFilterContext, string> getUserUniqueIdentity)
        {
            this.getUserUniqueIdentity = getUserUniqueIdentity ?? throw new ArgumentNullException(nameof(getUserUniqueIdentity));
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (true
                && context.ActionDescriptor.FilterDescriptors.Any(_ => _ is IAuthorizeData)
                && !context.ActionDescriptor.FilterDescriptors.Any(_ => _ is IAllowAnonymous)
                && context.HttpContext.User.Identity.IsAuthenticated)
            {
                var tokenUniqueId = context.HttpContext.User.Claims
                    .SingleOrDefault(_ => _.Type == ClaimType_UniqueIdentity)?.Value ?? Guid.NewGuid().ToString("N");
                if (tokenUniqueId == null)
                    goto Unauthorized;

                var userUniqueId = getUserUniqueIdentity.Invoke(context);
                if (tokenUniqueId != userUniqueId)
                    goto Unauthorized;

                Unauthorized:
                context.Result = new StatusCodeResult(401);
            }
        }
    }
}
