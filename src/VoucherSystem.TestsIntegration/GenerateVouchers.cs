using System.Net.Http;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using VoucherSystem.Dtos;

namespace VoucherSystem.TestsIntegration;

public class GenerateVouchers
{
    Regex vaucherRegex = new("^[A-Z0-9]+$");

    [Fact]
    public async Task ShouldGenerateVouchers()
    {
        int voucherLength = 12;
        int numberOfVouchersNeeded = 100;
    

        string? urlToWebApiFromEnv = Environment.GetEnvironmentVariable("URL_TO_WEB_API");
        Uri urlToWebApi = new Uri(urlToWebApiFromEnv!);

        HttpClient client = new();
        client.BaseAddress = urlToWebApi;

        Vouchers? response = await client.GetFromJsonAsync<Vouchers>($"generate-vouchers?voucherLength={voucherLength}&numberOfVouchersNeeded=${numberOfVouchersNeeded}");

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
