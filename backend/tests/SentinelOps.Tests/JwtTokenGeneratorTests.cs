using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using SentinelOps.Domain.Entities;
using SentinelOps.Infrastructure.Identity;
using Xunit;

namespace SentinelOps.Tests;

public class JwtTokenGeneratorTests
{
    [Fact]
    public void GenerateAccessToken_ShouldIncludeUserClaimsAndPermissions()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Secret"] = "ThisIsATestSecretKey1234567890!",
                ["Jwt:Issuer"] = "SentinelOpsAPI",
                ["Jwt:Audience"] = "SentinelOpsClient",
                ["Jwt:ExpiryMinutes"] = "60"
            })
            .Build();

        var user = new User
        {
            Id = 42,
            Username = "admin",
            Email = "admin@sentinelops.com",
            FullName = "System Administrator",
            Role = new Role { Name = "Administrator" }
        };

        var tokenGenerator = new JwtTokenGenerator(configuration);
        var token = tokenGenerator.GenerateAccessToken(user, new[] { "machines:read", "users:write" });

        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

        Assert.Equal("42", jwtToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
        Assert.Equal("admin", jwtToken.Claims.First(c => c.Type == ClaimTypes.Name).Value);
        Assert.Contains(jwtToken.Claims, claim => claim.Type == "permission" && claim.Value == "machines:read");
        Assert.Contains(jwtToken.Claims, claim => claim.Type == "permission" && claim.Value == "users:write");
    }
}
