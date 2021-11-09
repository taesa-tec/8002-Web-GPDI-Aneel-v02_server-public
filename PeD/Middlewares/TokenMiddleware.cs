using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Net.Http.Headers;

namespace PeD.Middlewares
{
    public class TokenMiddleware
    {
        private IDistributedCache _cache;
        private readonly RequestDelegate _next;

        public TokenMiddleware(IDistributedCache cache, RequestDelegate next)
        {
            _cache = cache;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                await _next(context);
                return;
            }

            if (context.Request.Headers.ContainsKey(HeaderNames.Authorization))
            {
                var headerAuthorization = context.Request.Headers[HeaderNames.Authorization].Single().Split(" ").Last();
                var value = _cache.Get(headerAuthorization);
                if (value != null)
                {
                    await _next(context);
                    return;
                }

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
            else
            {
                await _next(context);
            }
        }
    }
}