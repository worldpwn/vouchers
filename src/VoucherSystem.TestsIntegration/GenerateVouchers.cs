using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using VoucherSystem.Dtos;
using VoucherSystem.ValueObjects;
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
        int numberOfVouchersNeeded = 1000;
        string marketingCampaignName = "2023SpecialOffer";

        Uri urlToGenerateVouchers = new Uri($"{Config.GetUrlToWebApiFromEnv(output)}/generate-vouchers/{marketingCampaignName}/?voucherLength={voucherLength}&numberOfVouchersNeeded={numberOfVouchersNeeded}");
        
        HttpClient client = new();

        Vouchers? response = await client.GetFromJsonAsync<Vouchers>(urlToGenerateVouchers);

        Assert.NotNull(response);
        Assert.Equal(numberOfVouchersNeeded, response.amount);
        Assert.Equal(marketingCampaignName, response.marketingCampaignName);

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
