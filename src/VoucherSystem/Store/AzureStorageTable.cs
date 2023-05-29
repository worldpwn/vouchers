using System;
using Azure.Data.Tables;
using Azure.Identity;
using Microsoft.Extensions.Configuration;

namespace VoucherSystem.Store;

public class AzureStorageTable
{
    /// <summary>
    /// Will return table based on table name. If table doesn't exist it will create one.
    /// </summary>
    public async Task<TableClient> GetTableClient(string tableName)
    {
        string? url = Environment.GetEnvironmentVariable("STORAGE_ACCOUNT_NAME");
        if (url is null) throw new Exception("Cannot create Table Client because environment variable STORAGE_ACCOUNT_NAME is null");

        var tableClient = new TableClient(
                        new Uri("https://vouchersyst.table.core.windows.net"),
                        tableName,
                        new DefaultAzureCredential());
        await tableClient.CreateIfNotExistsAsync();
        return tableClient;
    }
}

