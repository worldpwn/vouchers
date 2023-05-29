using System.Collections.Generic;
using VoucherSystem.Dtos;
using VoucherSystem.Generator;
using Swashbuckle.AspNetCore.Annotations;
using VoucherSystem.Apis;
using VoucherSystem.Store;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o => o.EnableAnnotations());

builder.Services.AddSingleton<IGenerateRandomSymbol, GenerateRandomSymbol>();
builder.Services.AddScoped<GenerateVoucher>();
builder.Services.AddScoped<VouchersApi>();
builder.Services.AddSingleton<AzureStorageTable>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapGet("generate-vouchers",
    [SwaggerOperation(
    Summary = "Generate random `vouchers`.",
    Description = "Will generate random `vouchers` based on the needed length of the `voucher` and the number of `vouchers` needed. For `voucherLength` recommended value is 8+.")]
async (int voucherLength, int numberOfVouchersNeeded, VouchersApi vouchersApi)
=> await vouchersApi.GenerateRandomUniqueVouchers(voucherLength: voucherLength, numberOfVouchersNeeded: numberOfVouchersNeeded))
.WithOpenApi();

app.Run();