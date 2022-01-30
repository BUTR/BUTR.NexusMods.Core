using System.Text;
using BUTR.NexusMods.Server.Core.Helpers;
using BUTR.NexusMods.Server.Core.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace BUTR.NexusMods.Server.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServerCore(this IServiceCollection services, IConfiguration configuration, string jwtSectionName = "Jwt")
        {
            services.AddScoped<NexusModsAPIClient>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var jwtOptions = configuration.GetSection(jwtSectionName).Get<JwtOptions>();
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = false,
                        ValidateActor = false,
                        ValidateTokenReplay = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SignKey)),
                        TokenDecryptionKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.EncryptionKey)),
                        ClockSkew = TimeSpan.FromMinutes(5),
                    };
                });

            return services;
        }
    }
}