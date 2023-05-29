using System.Collections.Generic;
using VoucherSystem.Dtos;
using VoucherSystem.Generator;
using Swashbuckle.AspNetCore.Annotations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o => o.EnableAnnotations());

builder.Services.AddSingleton<IGenerateRandomSymbol, GenerateRandomSymbol>();
builder.Services.AddScoped<GenerateVoucher>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapGet("generate-vouchers",
[SwaggerOperation(
    Summary = "Generate random `vouchers`.",
    Description = "Will generate random `vouchers` based on the needed length of the `voucher` and the number of `vouchers` needed.")]
(int voucherLength, int numberOfVouchersNeeded, GenerateVoucher generateVoucher) =>
{
    HashSet<string> generatedVouchers = generateVoucher.GenerateRandomUniqueVouchers(voucherLength, numberOfVouchersNeeded);
    Vouchers vouchers = new Vouchers(string.Join(",", generatedVouchers), numberOfVouchersNeeded);
    return vouchers;
})
.WithOpenApi();

app.Run();