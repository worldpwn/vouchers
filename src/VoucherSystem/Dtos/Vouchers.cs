using System;
using VoucherSystem.ValueObjects;

namespace VoucherSystem.Dtos;

public record Vouchers(string vouchers, string marketingCampaignName, int amount);

