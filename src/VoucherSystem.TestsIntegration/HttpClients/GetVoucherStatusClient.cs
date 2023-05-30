using System;
using System.Net.Http.Json;
using VoucherSystem.Dtos;
using Xunit.Abstractions;

namespace VoucherSystem.TestsIntegration.HttpClients;

public static class GetVoucherStatusClient
{
    private static HttpClient client = StaticHttpClient.shared;

    public static async Task<VoucherStatus> GetVoucherStatus(ITestOutputHelper output, string marketingCampaignName, string voucher)
    {
        Uri urlToGetVoucherStatus = new Uri($"{Config.GetUrlToWebApiFromEnv(output)}/voucher-status/{marketingCampaignName}/{voucher}");
        VoucherStatus? response = await client.GetFromJsonAsync<VoucherStatus>(urlToGetVoucherStatus);
        if (response is null) throw new Exception($"Voucher is null from {urlToGetVoucherStatus}");
        return response;
    }
}

