using System;
using System.Text.RegularExpressions;

namespace VoucherSystem.ValueObjects
{
	public record MarketingCampaignName()
	{
        private string _value = string.Empty;
        public required string Value {
            get => _value;
            set
            {
                Regex regex = new("^[a-zA-Z\\d]+$");
                bool isMatch = regex.IsMatch(value);
                if (!isMatch) throw new ArgumentException("You can use only small letters and numbers.");
                this._value = value;
            }
        }
        public string ToTableName() => $"t_{Value}";

        public override string ToString() => this.Value;
    }
}

