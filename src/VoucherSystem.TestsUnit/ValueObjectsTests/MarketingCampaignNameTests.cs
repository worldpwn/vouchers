﻿using System;
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
        public void CorrectDataShouldReturnValue(string value)
        {
            MarketingCampaignName marketingCampaignName = new MarketingCampaignName() { Value = value};
            Assert.Equal(value, marketingCampaignName.ToString());
        }

        [Theory]
        [InlineData("summer_Holidays_Offer")]
        [InlineData("sale-for-partner")]
        [InlineData("@")]
        [InlineData("!")]
        [InlineData("?")]
        [InlineData("[")]
        public void InCorrectDataShouldReturnThrow(string value)
        {
            Action action = () => new MarketingCampaignName() { Value = value };
            Assert.Throws<ArgumentException>(action);
        }

        [Theory]
        [InlineData("summerHolidaysOffer", "t_summerHolidaysOffer")]
        [InlineData("saleForPartner", "t_saleForPartner")]
        [InlineData("sale2023", "t_sale2023")]
        [InlineData("2023sale", "t_2023sale")]
        public void CorrectDataShouldTransformToTableName(string value, string asTableName)
        {
            MarketingCampaignName marketingCampaignName = new MarketingCampaignName() { Value = value };
            Assert.Equal(asTableName, marketingCampaignName.ToTableName());
        }
    }
}

