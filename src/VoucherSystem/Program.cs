using System.Collections.Generic;
using VoucherSystem.Dtos;
using VoucherSystem.Generator;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IGenerateRandomSymbol, GenerateRandomSymbol>();
builder.Services.AddScoped<GenerateVoucher>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("generate-vouchers", (int voucherLength, int numberOfVouchersNeeded, GenerateVoucher generateVoucher) =>
{
    HashSet<string> generatedVouchers = generateVoucher.GenerateRandomUniqueVouchers(voucherLength, numberOfVouchersNeeded);
    Vouchers vouchers = new Vouchers(string.Join(",", generatedVouchers), numberOfVouchersNeeded);
    return vouchers;
})
.WithName("GenerateVouchers")
.WithOpenApi();

app.Run();