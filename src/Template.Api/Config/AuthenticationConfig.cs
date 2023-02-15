using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Template.Api.Config;

public static class AuthenticationConfig
{
    public static void AddAuthenticationConfig(this IServiceCollection services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        IConfiguration? config = services.BuildServiceProvider().GetService<IConfiguration>();

        byte[] key = Convert.FromBase64String(config.GetValue<string>("Auth:AudienceSecret"));

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = config.GetValue<string>("Auth:Issuer"),
                ValidAudience = config.GetValue<string>("Auth:AudienceId"),
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };
        });
    }
}

