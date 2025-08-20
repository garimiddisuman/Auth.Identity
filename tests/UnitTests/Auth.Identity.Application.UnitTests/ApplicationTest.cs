namespace Auth.Identity.Application.UnitTests;

public class ApplicationTest
{
    [Fact]
    public void Test_Add()
    {
        // Act
        var result = Application.Add(1, 1);
        
        // Assert
        Assert.Equal(2, result);
    }
}