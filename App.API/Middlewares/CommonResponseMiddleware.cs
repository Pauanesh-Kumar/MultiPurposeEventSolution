using App.Application.DTOs;
using System.Text.Json;

namespace App.API.Middlewares
{
    public class CommonResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public CommonResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var originalBody = context.Response.Body;

            using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;
            await _next(context);

            if (context.Response.ContentType != null && context.Response.ContentType.Contains("application/json"))
            {
                memoryStream.Seek(0, SeekOrigin.Begin);
                var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();

                var responseObj
                    = new ApiResponseModelDto<object>(
                        success: context.Response.StatusCode is >= 200 and <= 299,
                        data: JsonSerializer.Deserialize<object>(responseBody)!,
                        message: context.Response.StatusCode is >= 200 and <= 299 ? "Request completed successfully": "Error Occured");

                var jsonResponse = JsonSerializer.Serialize(responseObj);
                context.Response.Body = originalBody;
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}