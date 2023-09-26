namespace Leonardo.Tests;

public class UnitTest1
{
    [Fact]
    public async void Test1()
    {
        var results = await Fibonacci.RunAsync(new[] { "40", "41", "42" });
        Assert.Equal(3, results.Count);
        Assert.Equal(165580141, results[0]);
        Assert.Equal(267914296, results[1]);
        Assert.Equal(433494437, results[2]);
    }
}