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

public class GetVoucherStatus
{
    private readonly ITestOutputHelper output;

    public GetVoucherStatus(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public async Task ShouldGetStatusForExistentVoucher()
    {
        string marketingCampaignName = "2023SpecialOffer";
        HttpClient client = new();

        // generate voucher
        Vouchers? vouchersFromResponse = await GenerateVouchersClient.GenerateVoucher(output, marketingCampaignName, 12, 3);

        string[] vouchersAsStrings = vouchersFromResponse!.vouchers.Split(",");

        string voucher = vouchersAsStrings[1];
        VoucherStatus? response = await GetVoucherStatusClient.GetVoucherStatus(output, marketingCampaignName, voucher);

        Assert.NotNull(response);
        Assert.True(response.exist);
        Assert.False(response.used);
    }

    [Fact]
    public async Task ShouldGetStatusForNotExistentVoucher()
    {
        string marketingCampaignName = "2023SpecialOfferNonExist";
        string voucher = "LALAL1231";
        Uri urlToGetVoucherStatus = new Uri($"{Config.GetUrlToWebApiFromEnv(output)}/voucher-status/{marketingCampaignName}/{voucher}");
        HttpClient client = new();

        VoucherStatus? response = await GetVoucherStatusClient.GetVoucherStatus(output, marketingCampaignName, voucher);

        Assert.NotNull(response);
        Assert.False(response.exist);
        Assert.False(response.used);
    }
}
