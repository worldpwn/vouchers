using System;
using VoucherSystem.Dtos;
using VoucherSystem.Generator;
using VoucherSystem.Store;
using VoucherSystem.ValueObjects;

namespace VoucherSystem.Apis;

public class VouchersApi
{
	private readonly GenerateVoucher generateVoucher;
    private readonly AzureStorageTable azureStorageTable;

    public VouchersApi(GenerateVoucher generateVoucher, AzureStorageTable azureStorageTable)
    {
        this.generateVoucher = generateVoucher;
        this.azureStorageTable = azureStorageTable;
    }

    public async Task<Vouchers> GenerateRandomUniqueVouchers(MarketingCampaignName marketingCampaignName, int voucherLength, int numberOfVouchersNeeded)
	{
        HashSet<string> generatedVouchers = generateVoucher.GenerateRandomUniqueVouchers(voucherLength, numberOfVouchersNeeded);
        Vouchers vouchers = new Vouchers(
            vouchers: string.Join(",", generatedVouchers),
            marketingCampaignName: marketingCampaignName.ToString(),
            amount: numberOfVouchersNeeded);
        return vouchers;
    }
}

