using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using VoucherSystem.Dtos;
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
        Uri urlToGenerateVouchers = new Uri($"{Config.GetUrlToWebApiFromEnv(output)}/generate-vouchers/{marketingCampaignName}/?voucherLength={12}&numberOfVouchersNeeded={3}");
        HttpResponseMessage? responseToGenerate = await client.PostAsync(urlToGenerateVouchers, null);
        Vouchers? vouchersFromResponse = await responseToGenerate.Content.ReadFromJsonAsync<Vouchers>();

        string[] vouchersAsStrings = vouchersFromResponse!.vouchers.Split(",");

        string voucher = vouchersAsStrings[1];
        Uri urlToGetVoucherStatus = new Uri($"{Config.GetUrlToWebApiFromEnv(output)}/voucher-status/{marketingCampaignName}/{voucher}");
        VoucherStatus? response = await client.GetFromJsonAsync<VoucherStatus>(urlToGetVoucherStatus);

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

        VoucherStatus? response = await client.GetFromJsonAsync<VoucherStatus>(urlToGetVoucherStatus);

        Assert.NotNull(response);
        Assert.False(response.exist);
        Assert.False(response.used);
    }
}
