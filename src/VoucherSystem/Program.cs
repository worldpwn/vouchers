using System.Collections.Generic;
using VoucherSystem.Dtos;
using VoucherSystem.Generator;
using Swashbuckle.AspNetCore.Annotations;
using VoucherSystem.Apis;
using VoucherSystem.Store;
using VoucherSystem.ValueObjects;
using Azure;
using System.Net;
using VoucherSystem.Exceptions;
using Microsoft.AspNetCore.Mvc;

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

app.MapPost("generate-vouchers/{marketingCampaignName}",
    [SwaggerOperation(
    Summary = "Generate random `vouchers`.",
    Description = "Will generate random `vouchers` based on the needed length of the `voucher` and the number of `vouchers` needed. Minimal `voucherLength` is 6.")]
async (int voucherLength, int numberOfVouchersNeeded, string marketingCampaignName, VouchersApi vouchersApi)
=> await vouchersApi.GenerateRandomUniqueVouchers(
    marketingCampaignName: new MarketingCampaignName() { Value = marketingCampaignName },
    voucherLength: voucherLength,
    numberOfVouchersNeeded: numberOfVouchersNeeded))
.WithOpenApi();

app.MapGet("voucher-status/{marketingCampaignName}/{voucher}",
    [SwaggerOperation(
    Summary = "Check if `voucher` is `used`, `new`, `not-valid`.",
    Description = "Will return `voucher` status.")]
async (string marketingCampaignName, string voucher, VouchersApi vouchersApi)
=> await vouchersApi.GetVoucherStatus(
    marketingCampaignName: new MarketingCampaignName() { Value = marketingCampaignName },
    voucher: voucher))
.WithOpenApi();

app.MapPut("use-voucher/{marketingCampaignName}/{voucher}",
    [SwaggerOperation(
    Summary = "Will use `voucher`.",
    Description = "Will set `used` variable to true. If voucher doesnt exist and if the voucher is already used will return Error")]
async (string marketingCampaignName, string voucher, VouchersApi vouchersApi)
=> await UseVoucher(marketingCampaignName, voucher, vouchersApi))
.WithOpenApi();


app.Run();

static async Task<IResult> UseVoucher(string marketingCampaignName, string voucher, VouchersApi vouchersApi)
{
    try
    {
        VoucherStatus voucherStatus = await vouchersApi.UseVoucher(
            marketingCampaignName: new MarketingCampaignName() { Value = marketingCampaignName },
            voucher: voucher);
        return Results.Ok(voucherStatus);
    }
    catch (VoucherDoesntExistException ex)
    {
        return Results.BadRequest(ex.Message);
    }
    catch (VoucherUsedException ex)
    {
        return Results.BadRequest(ex.Message);
    }
}