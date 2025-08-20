namespace Auth.Identity.Domain.UnitTests;

public class DomainTests
{
    [Fact]
    public void Test_Sub()
    {
        // Act
        var result = Domain.Sub(1, 1);
        
        // Assert
        Assert.Equal(2, result);
    }
}