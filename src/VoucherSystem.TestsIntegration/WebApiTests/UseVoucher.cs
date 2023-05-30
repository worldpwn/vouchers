using System;
using System.Net;
using VoucherSystem.Dtos;
using VoucherSystem.TestsIntegration.HttpClients;
using VoucherSystem.ValueObjects;
using Xunit.Abstractions;

namespace VoucherSystem.TestsIntegration.WebApiTests
{
	public class UseVoucher
	{
        private readonly ITestOutputHelper output;

        public UseVoucher(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public async Task ShouldMarkVoucherAsUsedWhenVoucherIsUsedAndVoucherExist()
        {
            string marketingCampaignName = "2023SpecialOffer";

            // generate voucher
            Vouchers? vouchersFromResponse = await GenerateVouchersClient.GenerateVoucher(output, marketingCampaignName, 12, 3);
            string[] vouchersAsStrings = vouchersFromResponse!.vouchers.Split(",");
            string voucher = vouchersAsStrings[1];

            // Act
            VoucherStatus voucherStatus = await UseVoucherClient.UseVoucher(output, marketingCampaignName, voucher);

            // Assert
            Assert.True(voucherStatus.exist);
            Assert.True(voucherStatus.used);
        }

        [Fact]
        public async Task ShouldReturnBadRequestVoucherDoesntExist()
        {
            string marketingCampaignName = "2023SpecialOfferNo";

            // Act
            HttpResponseMessage responseMessage = await UseVoucherClient.UseVoucherExpectException(output, marketingCampaignName, "RANDOM");

            // Assert
            Assert.False(responseMessage.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, responseMessage.StatusCode);
        }

        [Fact]
        public async Task ShouldReturnBadRequestVoucherIsUsed()
        {
            string marketingCampaignName = "2023SpecialOffer";

            // generate voucher
            Vouchers? vouchersFromResponse = await GenerateVouchersClient.GenerateVoucher(output, marketingCampaignName, 12, 3);
            string[] vouchersAsStrings = vouchersFromResponse!.vouchers.Split(",");
            string voucher = vouchersAsStrings[1];
            // use once
            await UseVoucherClient.UseVoucher(output, marketingCampaignName, voucher);

            // Act
            HttpResponseMessage responseMessage = await UseVoucherClient.UseVoucherExpectException(output, marketingCampaignName, voucher);

            // Assert
            Assert.False(responseMessage.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, responseMessage.StatusCode);
        }
    }
}

