namespace APIGateWay
{
    public class Gatewaymiddleware
    {
        private readonly RequestDelegate _requestDelegate;

        public Gatewaymiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.Headers["referrer"] = "api-gateway";
            await _requestDelegate(context);
        }
    }
}
