using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Template.Api.Config;

[ExcludeFromCodeCoverage]
public static class ApiVersioningConfiguration
{
    public static void AddVersioningConfig(this IServiceCollection services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        services.AddApiVersioning(opt =>
        {
            opt.DefaultApiVersion = new ApiVersion(1, 0);
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.ApiVersionReader = new UrlSegmentApiVersionReader();
            opt.ReportApiVersions = true;
        });

        services.AddVersionedApiExplorer(opt =>
        {
            opt.GroupNameFormat = "'v'VVV";

            opt.SubstituteApiVersionInUrl = true;
        });
    }
}