using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using VoucherSystem.Dtos;
using VoucherSystem.TestsIntegration.HttpClients;
using VoucherSystem.ValueObjects;
using Xunit.Abstractions;

namespace VoucherSystem.TestsIntegration.WebApiTests;

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

        // Act
        Vouchers? vouchersFromResponse = await GenerateVouchersClient.GenerateVoucher(output, marketingCampaignName, voucherLength, numberOfVouchersNeeded);

        // Assert
        Assert.NotNull(vouchersFromResponse);
        Assert.Equal(numberOfVouchersNeeded, vouchersFromResponse.amount);
        Assert.Equal(marketingCampaignName, vouchersFromResponse.marketingCampaignName);

        // validate vouchers
        string[] vouchersItslef = vouchersFromResponse.vouchers.Split(',');
        Assert.Equal(numberOfVouchersNeeded, vouchersItslef.Length);
        foreach (string voucher in vouchersItslef)
        {
            Assert.Equal(voucherLength, voucher.Length);
            Assert.True(vaucherRegex.Match(voucher).Success);
        }
    }
}
