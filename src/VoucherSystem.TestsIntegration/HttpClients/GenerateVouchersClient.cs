using System;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System.Net.Http.Json;
using VoucherSystem.Dtos;
using VoucherSystem.ValueObjects;
using Xunit.Abstractions;

namespace VoucherSystem.TestsIntegration.HttpClients;

public static class GenerateVouchersClient
{
    public static async Task<Vouchers> GenerateVoucher(ITestOutputHelper output, string marketingCampaignName, int voucherLength, int numberOfVouchersNeeded)
    {
        Uri urlToGenerateVouchers = new Uri($"{Config.GetUrlToWebApiFromEnv(output)}/generate-vouchers/{marketingCampaignName}/?voucherLength={voucherLength}&numberOfVouchersNeeded={numberOfVouchersNeeded}");
        HttpClient client = new();

        HttpResponseMessage? response = await client.PostAsync(urlToGenerateVouchers, null);
        response.EnsureSuccessStatusCode();
        Vouchers? vouchersFromResponse = await response.Content.ReadFromJsonAsync<Vouchers>();
        if (vouchersFromResponse is null) throw new Exception($"Voucher is null from {urlToGenerateVouchers}");
        return vouchersFromResponse;
    }
}

