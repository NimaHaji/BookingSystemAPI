using Application.Features.Tenant.Interfaces;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace api.Middlewares;

public class TenantResolutionMiddleware
{
    private readonly RequestDelegate _next;

    public TenantResolutionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context,
        TenantProviderContract tenantProvider,       
        TenantRepositoryContract tenantRepository)
    {
        var path = context.Request.Path.Value?.ToLower();
        if (string.IsNullOrEmpty(path) ||
            path.StartsWith("/swagger") ||
            path.StartsWith("/health") ||
            path.StartsWith("/api/") ||
            path == "/")
        {
            await _next(context);
            return;
        }
        Guid tenantId = Guid.Empty;

        // روش ۳: از JWT claim (برای درخواست‌های احراز هویت شده)
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var claim = context.User.FindFirst("TenantId")?.Value;
            Guid.TryParse(claim, out tenantId);
        }

        // اگر JWT وجود نداشت، می‌توانید از روش‌های دیگر (هدر یا ساب‌دامین) استفاده کنید
        // که خودتان کامنت کرده‌اید.

        if (tenantId == Guid.Empty)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("TenantId is missing");
            return;
        }

        tenantProvider.SetTenantId(tenantId);

        // بررسی اعتبار اشتراک Tenant
        var isValid = await tenantRepository.IsSubscriptionValidAsync();
        if (!isValid)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Subscription is inactive or expired");
            return;
        }

        await _next(context);
    }
}