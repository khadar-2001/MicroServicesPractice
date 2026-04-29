using Microsoft.AspNetCore.Http;
namespace MiddlewareLib
{
    public class APIInterceptor
    {
        private RequestDelegate _requestDelegate;

        public APIInterceptor(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string referrer = context.Request.Headers["referrer"].ToString();

            if (string.IsNullOrEmpty(referrer))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;

                await context.Response.WriteAsync("You are not authorized to call this API Directly");
                return;
            }
            else
                await _requestDelegate(context);
        }
        
    }
}
