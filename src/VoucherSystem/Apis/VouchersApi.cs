using System;
using VoucherSystem.Dtos;
using VoucherSystem.Generator;
using VoucherSystem.Store;

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

    public async Task<Vouchers> GenerateRandomUniqueVouchers(int voucherLength, int numberOfVouchersNeeded)
	{
        HashSet<string> generatedVouchers = generateVoucher.GenerateRandomUniqueVouchers(voucherLength, numberOfVouchersNeeded);
        Vouchers vouchers = new Vouchers(string.Join(",", generatedVouchers), numberOfVouchersNeeded);
        return vouchers;
    }
}

