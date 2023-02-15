using Microsoft.AspNetCore.Mvc;
using Template.Infra.Data.Oracle;
using Template.Infra.Data.SqlServer;
using Template.Infra.Proxy.Shared;
using Template.Application.Profiles;
using Template.Application.Services.Template;
using Template.Application.Shared.Notifications;

namespace Template.Api.Config;

public static class DependencyInjectionConfig
{
    public static void AddDependencyInjection(this IServiceCollection services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        services.AddScoped<ITemplateProxyService, TemplateProxyService>();
        services.AddSingleton<ITemplateAuthenticationService, TemplateAuthenticationService>();

        services.AddAutoMapper(typeof(BaseProfile).Assembly);

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        services.AddScoped<ISqlServerUnitOfWork, SqlServerUnitOfWork>();
        services.AddScoped<IOracleUnitOfWork, OracleUnitOfWork>();
        services.AddScoped<ITemplateService, TemplateService>();
        services.AddScoped<NotificationContext>();
    }
}


