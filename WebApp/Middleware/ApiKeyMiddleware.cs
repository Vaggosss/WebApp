public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private const string APIKEY_HEADER_NAME = "X-API-Key";
    private readonly string _apiKey;

    public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _apiKey = configuration.GetValue<string>("ApiKey"); // Θα το ορίσουμε στο appsettings.json
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(APIKEY_HEADER_NAME, out var extractedApiKey))
        {
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("API Key is missing");
            return;
        }

        if (!_apiKey.Equals(extractedApiKey))
        {
            context.Response.StatusCode = 403; // Forbidden
            await context.Response.WriteAsync("Unauthorized client");
            return;
        }

        await _next(context);
    }
}
