using System;
using System.Linq;

using Microsoft.AspNetCore.Http;

namespace Liyanjie.AspNetCore.Http.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetClientIpAddress(this HttpContext httpContext)
        {
            string ipAddr = null;

            if (httpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var xForwardedFor))
                ipAddr = ((string)xForwardedFor).Split(',')
                    .Select(_ => _.Trim())
                    .FirstOrDefault();

            if (ipAddr.IsNullOrEmpty())
                ipAddr = httpContext.Connection?.RemoteIpAddress?.ToString();

            if (ipAddr.IsNullOrEmpty())
                if (httpContext.Request.Headers.TryGetValue("REMOTE_ADDR", out var remoteAddr))
                    ipAddr = remoteAddr;

            return ipAddr;
        }
    }
}
