using System;
using VoucherSystem.Exceptions;
using VoucherSystem.Generator;
using VoucherSystem.TestsUnit.Mock;

namespace VoucherSystem.TestsUnit.GeneratorTests;

public class GenerateVoucherTests
{
	[Fact]
	public void ShouldGenerateVoucherFilledWithDesiredSymbol()
	{
        GenerateRandomSymbolMock generateRandomSymbolMock = new GenerateRandomSymbolMock('A');
        GenerateVoucher generateVoucher = new(generateRandomSymbolMock);
        HashSet<string> vouchers = generateVoucher.GenerateRandomUniqueVouchers(4, 1);

        Assert.Single(vouchers);
        Assert.Equal("AAAA", vouchers.First());
    }

    [Fact]
    public void ShouldGenerateNeededNumberOfVouchers()
    {
        int numberOfVouchers = 1000;
        int lenghtOfVouchers = 6;
        GenerateVoucher generateVoucher = new(new GenerateRandomSymbol());
        HashSet<string> vouchers = generateVoucher.GenerateRandomUniqueVouchers(lenghtOfVouchers, numberOfVouchers);

        Assert.Equal(numberOfVouchers, vouchers.Count());
    }

    [Fact]
    public void ShouldThrowDomainErrorWhenSameVoucherIsGenerated()
    {
        int numberOfVouchers = 2;
        int lenghtOfVouchers = 6;
        GenerateRandomSymbolMock generateRandomSymbolMock = new GenerateRandomSymbolMock('A');
        GenerateVoucher generateVoucher = new(generateRandomSymbolMock);
        Action action = () => generateVoucher.GenerateRandomUniqueVouchers(lenghtOfVouchers, numberOfVouchers);
        Assert.Throws<CannotGenerateUniqueVoucherExceptions>(action);
    }
}

