using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SentinelOps.Application.Common.Interfaces;
using SentinelOps.Domain.Repositories;
using SentinelOps.Infrastructure.Identity;
using SentinelOps.Infrastructure.Persistence;
using SentinelOps.Infrastructure.Persistence.Repositories;
using SentinelOps.Infrastructure.Services;

namespace SentinelOps.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? "Server=localhost;Port=3306;Database=sentinelops_db;Uid=sentinelops_user;Pwd=user_password_123;AllowPublicKeyRetrieval=True;SslMode=Preferred;";
        var databaseProvider = configuration["DatabaseProvider"] ?? "MySql";

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            if (databaseProvider.Equals("Sqlite", StringComparison.OrdinalIgnoreCase) || connectionString.StartsWith("Data Source=", StringComparison.OrdinalIgnoreCase))
            {
                options.UseSqlite(connectionString,
                    sqlite => sqlite.MigrationsAssembly("SentinelOps.Presentation"));
                return;
            }

            options.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString),
                mySql => mySql.MigrationsAssembly("SentinelOps.Presentation"));
        });

        services.AddScoped<IMachineRepository, MachineRepository>();
        services.AddScoped<IMachineTypeRepository, MachineTypeRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddHttpContextAccessor();
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        var secret = configuration["Jwt:Secret"] ?? "SentinelOpsSuperSecretKey123!@#ChangeMeInProd";
        var issuer = configuration["Jwt:Issuer"] ?? "SentinelOpsAPI";
        var audience = configuration["Jwt:Audience"] ?? "SentinelOpsClient";

        var signingKeyBytes = Encoding.UTF8.GetBytes(secret);
        if (signingKeyBytes.Length < 32)
        {
            using var sha256 = SHA256.Create();
            signingKeyBytes = sha256.ComputeHash(signingKeyBytes);
        }

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes),
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddAuthorization(options =>
        {
            foreach (var permission in new[]
            {
                "machines:read",
                "machines:write",
                "machines:delete",
                "users:read",
                "users:write",
                "audit_logs:read"
            })
            {
                options.AddPolicy(permission, policy =>
                    policy.RequireAssertion(context =>
                        context.User.HasClaim(c => c.Type == "permission" && c.Value == permission)));
            }
        });

        return services;
    }
}
