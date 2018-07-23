using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Middleware
{
    public class ForceCultureMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly CultureInfo _cultureInfo = new CultureInfo("en-US");
        public ForceCultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task InvokeAsync(HttpContext context)
        {
            CultureInfo.CurrentCulture = _cultureInfo;
            CultureInfo.CurrentUICulture = _cultureInfo;
            return _next(context);
        }
    }
}
