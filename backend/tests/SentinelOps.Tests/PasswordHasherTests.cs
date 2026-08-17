using Xunit;
using SentinelOps.Shared.Utilities;

namespace SentinelOps.Tests;

public class PasswordHasherTests
{
    [Fact]
    public void HashPassword_ShouldVerifyOriginalPassword()
    {
        var password = "AdminPassword123!";
        var hash = PasswordHasher.HashPassword(password);

        Assert.NotEqual(password, hash);
        Assert.True(PasswordHasher.VerifyPassword(password, hash));
        Assert.False(PasswordHasher.VerifyPassword("wrong-password", hash));
    }
}
