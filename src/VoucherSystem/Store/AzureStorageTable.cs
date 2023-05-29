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
        TableClient tableClient = new TableClient(
                        new Uri($"https://{storageAccountName}.table.core.windows.net"),
                        tableName,
                        new DefaultAzureCredential());
        await tableClient.CreateIfNotExistsAsync();
        return tableClient;
    }
}

