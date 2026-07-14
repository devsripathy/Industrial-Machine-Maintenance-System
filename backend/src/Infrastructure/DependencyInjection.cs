using System;
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

namespace SentinelOps.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? "Server=localhost;Port=3306;Database=sentinelops_db;Uid=root;Pwd=root_password_123;";

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
            ));

        services.AddScoped<IMachineRepository, MachineRepository>();
        services.AddScoped<IMachineTypeRepository, MachineTypeRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        // Configure Authentication
        var secret = configuration["Jwt:Secret"] ?? "SentinelOpsSuperSecretKey123!@#ChangeMeInProd";
        var issuer = configuration["Jwt:Issuer"] ?? "SentinelOpsAPI";
        var audience = configuration["Jwt:Audience"] ?? "SentinelOpsClient";

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
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                ClockSkew = TimeSpan.Zero
            };
        });

        return services;
    }
}
