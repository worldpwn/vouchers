namespace VoucherSystem.TestsIntegration;

public class GenerateVouchers
{
    [Fact]
    public void ShouldGenerateVouchers()
    {
        var urlToWebApi = Environment.GetEnvironmentVariable("URL_TO_WEB_API");
        Assert.Equal("LOL", urlToWebApi);
    }
}
