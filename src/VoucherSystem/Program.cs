using System.Collections.Generic;
using VoucherSystem.Dtos;
using VoucherSystem.Generator;
using Swashbuckle.AspNetCore.Annotations;
using VoucherSystem.Apis;
using VoucherSystem.Store;
using VoucherSystem.ValueObjects;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o => o.EnableAnnotations());

builder.Services.AddSingleton<IGenerateRandomSymbol, GenerateRandomSymbol>();
builder.Services.AddScoped<GenerateVoucher>();
builder.Services.AddScoped<VouchersApi>();
builder.Services.AddSingleton<AzureStorageTable>(o =>
    new AzureStorageTable(builder.Configuration.GetValue<string>("STORAGE_ACCOUNT_NAME") ?? throw new Exception("Cannot create Table Client because variable STORAGE_ACCOUNT_NAME is null")));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();  

app.MapGet("generate-vouchers/{marketingCampaignName}",
    [SwaggerOperation(
    Summary = "Generate random `vouchers`.",
    Description = "Will generate random `vouchers` based on the needed length of the `voucher` and the number of `vouchers` needed. Minimal `voucherLength` is 6.")]
async (int voucherLength, int numberOfVouchersNeeded, string marketingCampaignName, VouchersApi vouchersApi)
=> await vouchersApi.GenerateRandomUniqueVouchers(
    marketingCampaignName: new MarketingCampaignName() { Value = marketingCampaignName },
    voucherLength: voucherLength,
    numberOfVouchersNeeded: numberOfVouchersNeeded))
.WithOpenApi();

app.Run();