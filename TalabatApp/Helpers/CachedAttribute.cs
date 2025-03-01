using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using TalabatApp.Core.Services.Contract;

namespace TalabatApp.Helpers
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _expireTimeInSeconds;

        public CachedAttribute(int ExpireTimeInSeconds)
        {
            _expireTimeInSeconds = ExpireTimeInSeconds;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var CacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
            var CacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

            var CacheResponse = await CacheService.GetCachedResponse(CacheKey);

            if (!string.IsNullOrEmpty(CacheResponse))
            {
                var contentResult = new ContentResult()
                {
                    Content = CacheResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }

            var ExecutedEndpoint = await next.Invoke();

            if(ExecutedEndpoint.Result is OkObjectResult result)
            {
               await CacheService.CacheResponseAsync(CacheKey, result.Value, TimeSpan.FromSeconds(_expireTimeInSeconds));

            }
        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var KeyBuilder = new StringBuilder();
            KeyBuilder.Append(request.Path);

            foreach (var (Key, Value) in request.Query.OrderBy(x => x.Key))
            {
                KeyBuilder.Append($"|{Key}-{Value}");
            }
            return KeyBuilder.ToString();
        }
    }
}
