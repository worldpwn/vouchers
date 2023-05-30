using System;
using System.Net.Http.Json;
using VoucherSystem.Dtos;
using Xunit.Abstractions;

namespace VoucherSystem.TestsIntegration.HttpClients;

public static class UseVoucherClient
{
    private static HttpClient client = StaticHttpClient.shared;

    public static async Task<VoucherStatus> UseVoucher(ITestOutputHelper output, string marketingCampaignName, string voucher)
    {
        Uri url = new Uri($"{Config.GetUrlToWebApiFromEnv(output)}/use-voucher/{marketingCampaignName}/{voucher}");
        HttpResponseMessage? response = await client.PutAsync(url, null);
        response.EnsureSuccessStatusCode();
        VoucherStatus? dto = await response.Content.ReadFromJsonAsync<VoucherStatus>();
        if (dto is null) throw new Exception($"Voucher is null from {url}");
        return dto;
    }

    public static async Task<HttpResponseMessage> UseVoucherExpectException(ITestOutputHelper output, string marketingCampaignName, string voucher)
    {
        Uri url = new Uri($"{Config.GetUrlToWebApiFromEnv(output)}/use-voucher/{marketingCampaignName}/{voucher}");
        HttpResponseMessage? response = await client.PutAsync(url, null);
        if (response is null) throw new Exception($"HttpResponseMessage is null from {url}");
        return response;
    }
}
