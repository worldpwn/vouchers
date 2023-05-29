using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text.Json;
using System.Transactions;
using Azure;
using Azure.Data.Tables;
using VoucherSystem.Dtos;
using VoucherSystem.Generator;
using VoucherSystem.Store;
using VoucherSystem.ValueObjects;

namespace VoucherSystem.Apis;

public class VouchersApi
{
    public const string voucherTableName = "Vouchers";
    public const int maximumNumberPerBatch = 100;

    private readonly GenerateVoucher generateVoucher;
    private readonly AzureStorageTable azureStorageTable;

    public VouchersApi(GenerateVoucher generateVoucher, AzureStorageTable azureStorageTable)
    {
        this.generateVoucher = generateVoucher;
        this.azureStorageTable = azureStorageTable;
    }

    public async Task<Vouchers> GenerateRandomUniqueVouchers(MarketingCampaignName marketingCampaignName, int voucherLength, int numberOfVouchersNeeded)
    {
        if (voucherLength < 6) throw new ArgumentException("Minimal voucherLength is 6");

        HashSet<string> generatedVouchers = generateVoucher.GenerateRandomUniqueVouchers(voucherLength, numberOfVouchersNeeded);

        TableClient tableClient = await azureStorageTable.GetTableClient(voucherTableName);
        await VoucherBatchAdd(tableClient, marketingCampaignName, generatedVouchers);

        Vouchers vouchers = new Vouchers(
           vouchers: string.Join(",", generatedVouchers),
           marketingCampaignName: marketingCampaignName.ToString(),
           amount: numberOfVouchersNeeded);
        return vouchers;
    }

    public async Task<VoucherStatus> GetVoucherStatus(MarketingCampaignName marketingCampaignName, string voucher)
    {
        TableClient tableClient = await azureStorageTable.GetTableClient(voucherTableName);
        List<JsonDocument> result = new();
        string filter = $"PartitionKey eq '{marketingCampaignName}' and RowKey eq '{voucher}'";
        AsyncPageable<TableEntity> queryResultsFilter = tableClient.QueryAsync<TableEntity>(filter: filter);
        bool exist = false;
        bool used = false;
        await foreach (TableEntity qEntity in queryResultsFilter)
        {
            exist = true;
            used = qEntity.GetBoolean("Used") ?? throw new Exception($"Entity has not Used field for filter {filter}");
        }
        return new VoucherStatus(exist: exist, used: used);
    }

    private static async Task VoucherBatchAdd(TableClient tableClient, MarketingCampaignName marketingCampaignName, HashSet<string> vouchers)
    {
        List<Task> batchTasks = new List<Task>();
        List<TableEntity> entityList = vouchers.Select(v => new TableEntity(marketingCampaignName.ToString(), v)   {
            { "Used", false },
        }).ToList();
        for (int i = 0; i < entityList.Count; i += maximumNumberPerBatch)
        {
            IEnumerable<TableEntity> entity = entityList.Skip(i).Take(maximumNumberPerBatch);
            Task batchTask = SubmitTransaction(tableClient, entity);
            i += maximumNumberPerBatch;
            batchTasks.Add(batchTask);
        }
        await Task.WhenAll(batchTasks);
    }

    private static Task SubmitTransaction(TableClient tableClient, IEnumerable<TableEntity> entities)
    {
        List<TableTransactionAction> addEntitiesBatch = new List<TableTransactionAction>();
        // Add the entities to be added to the batch.
        addEntitiesBatch.AddRange(entities.Select(e => new TableTransactionAction(TableTransactionActionType.Add, e)));
        return tableClient.SubmitTransactionAsync(addEntitiesBatch);
    }
}

