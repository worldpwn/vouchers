using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using VoucherSystem.Dtos;
using Xunit.Abstractions;

namespace VoucherSystem.TestsIntegration;

public class GenerateVouchers
{
    private readonly Regex vaucherRegex = new("^[A-Z0-9]+$");
    private readonly ITestOutputHelper output;

    public GenerateVouchers(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public async Task ShouldGenerateVouchers()
    {
        int voucherLength = 12;
        int numberOfVouchersNeeded = 100;
    

        string? urlToWebApiFromEnv = Environment.GetEnvironmentVariable("URL_TO_WEB_API");
        if (urlToWebApiFromEnv is null)
        {
            urlToWebApiFromEnv = "https://localhost:7148";
            output.WriteLine("Cannot find URL_TO_WEB_API enviornment variable");
            output.WriteLine($"Fallback to {urlToWebApiFromEnv}");
        }

        Uri urlToWebApi = new Uri($"{urlToWebApiFromEnv}/generate-vouchers?voucherLength={voucherLength}&numberOfVouchersNeeded={numberOfVouchersNeeded}");
        
        HttpClient client = new();

        Vouchers? response = await client.GetFromJsonAsync<Vouchers>(urlToWebApi);

        Assert.NotNull(response);
        Assert.Equal(numberOfVouchersNeeded, response.amount);

        // validate vouchers
        string[] vouchersItslef = response.vouchers.Split(',');
        Assert.Equal(numberOfVouchersNeeded, vouchersItslef.Length);
        foreach (string voucher in vouchersItslef)
        {
            Assert.Equal(voucherLength, voucher.Length);
            Assert.True(vaucherRegex.Match(voucher).Success);
        }
    }
}
