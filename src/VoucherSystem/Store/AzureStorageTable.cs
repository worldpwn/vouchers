using System;
using Azure.Data.Tables;
using Azure.Identity;
using Microsoft.Extensions.Configuration;

namespace VoucherSystem.Store;

public class AzureStorageTable
{
    private readonly string storageAccountName;
    public AzureStorageTable(string storageAccountName)
    {
        this.storageAccountName = storageAccountName;
    }

    /// <summary>
    /// Will return table based on table name. If table doesn't exist it will create one.
    /// </summary>
    public async Task<TableClient> GetTableClient(string tableName)
    {
        string connectionString = "DefaultEndpointsProtocol=https;AccountName=vouchersyst;AccountKey=Qk3EMQyPfvOi8U5G55YgUEzSMRt0XYW7uvQ66Ezc7AId7VUDCoVxFiijzSxcQLAffwx8e5/L1bT6+AStd49eTw==;EndpointSuffix=core.windows.net";
        TableClient tableClient = new TableClient(
                       connectionString,
                       tableName);

        //TableClient tableClient = new TableClient(
        //                new Uri($"https://{storageAccountName}.table.core.windows.net"),
        //                tableName,
        //                new DefaultAzureCredential());
        await tableClient.CreateIfNotExistsAsync();
        return tableClient;
    }
}

