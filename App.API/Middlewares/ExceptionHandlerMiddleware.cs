using System.Net;
using App.Application.CustomExceptions;
using App.Application.DTOs;
using Microsoft.AspNetCore.Http;
namespace App.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger,
            IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            // ── Existing custom exceptions (unchanged) ─────────────────────────────────────
            catch (InvalidCredentialsException ex) { await HandleBadRequest(context, ex, "Wrong Credentials Passed"); }
            catch (UnAuthorizedException ex) { await HandleUnauthorized(context, ex, "UnAuthorized"); }
            catch (RecordNotFoundException ex) { await HandleBadRequest(context, ex, "The record doesn't exists in database."); }
            catch (ArgumentException ex) { await HandleBadRequest(context, ex, "Bad Request"); }
            // ── NEW: Duplicate e-mail (the only change you asked for) ─────────────────────
            catch (InvalidOperationException ex) when (ex.Message.Contains("already registered", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning(ex, "Duplicate registration attempt");
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.Conflict;   // 409

                var problem = ApiErrorModelDto(
                    context,
                    ex,
                    (int)HttpStatusCode.Conflict,
                    "User already registered. Please check the Email!");

                await context.Response.WriteAsJsonAsync(problem);
            }

            // ── Fallback for any other unexpected exception ───────────────────────────────
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var problem = ApiErrorModelDto(
                    context,
                    ex,
                    (int)StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred!");

                await context.Response.WriteAsJsonAsync(problem);
            }
        }

        // ── Helper methods to keep the catch-blocks tidy ─────────────────────────────────
        private async Task HandleBadRequest(HttpContext ctx, Exception ex, string message)
        {
            _logger.LogError(ex, "Bad request");
            ctx.Response.ContentType = "application/json";
            ctx.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            var problem = ApiErrorModelDto(ctx, ex, (int)HttpStatusCode.BadRequest, message);
            await ctx.Response.WriteAsJsonAsync(problem);
        }

        private async Task HandleUnauthorized(HttpContext ctx, Exception ex, string message)
        {
            _logger.LogError(ex, "Unauthorized");
            ctx.Response.ContentType = "application/json";
            ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

            var problem = ApiErrorModelDto(ctx, ex, (int)HttpStatusCode.Unauthorized, message);
            await ctx.Response.WriteAsJsonAsync(problem);
        }
        private ApiErrorModelDto ApiErrorModelDto(
            HttpContext context,
            Exception ex,
            int statusCode,
            string title)
        {
            var problem = new ApiErrorModelDto()
            {
                StatusCode = statusCode,
                Title = title,
                Detail = ex.Message,
                StackTrace = _env.IsDevelopment() ? ex.ToString() : "Something went wrong.",
                Instance = context.Request.Path
            };
            return problem;
        }
    }
}