using System;
using VoucherSystem.ValueObjects;

namespace VoucherSystem.TestsUnit.ValueObjectsTests;

public class MarketingCampaignNameTests
{
    public class UrlName_ToString
    {
        [Theory]
        [InlineData("summerHolidaysOffer")]
        [InlineData("saleForPartner")]
        [InlineData("sale2023")]
        [InlineData("2023sale")]
        public void Correct_Data_Should_Return_Value(string value)
        {
            MarketingCampaignName marketingCampaignName = new MarketingCampaignName() { Value = value};
            Assert.Equal(value, marketingCampaignName.ToString());
        }
    }
}

