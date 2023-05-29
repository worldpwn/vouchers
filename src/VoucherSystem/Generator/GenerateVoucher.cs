using System;
using System.Runtime.InteropServices;
using System.Text;
using VoucherSystem.Exceptions;

namespace VoucherSystem.Generator;

public class GenerateVoucher
{
	private readonly IGenerateRandomSymbol generateRandomSymbol;
    public GenerateVoucher(IGenerateRandomSymbol generateRandomSymbol)
	{
		this.generateRandomSymbol = generateRandomSymbol;
	}

    /// <summary>
    /// Will try to generate random vouchers based on the needed length of the voucher and the number of vouchers needed. <br/>
    /// Because it will use random symbols please don't use too small a length of the voucher especially if you need a lot of vouchers. <br/>
    /// Recommendations: <br/>
    /// - 100 vouchers = 6 symbols, <br/>
    /// - 100,000 vouchers = 12 symbols,  <br/>
    /// - 1.000.000 vouchers = 15 symbols. <br/>
    /// </summary>
    /// <exception cref="CannotGenerateUniqueVoucherExceptions"></exception>
	public HashSet<string> GenerateRandomUniqueVouchers(int voucherLength, int numberOfVouchersNeeded)
	{
		HashSet<string> vouchers = new HashSet<string>();
        StringBuilder voucherBuilder = new StringBuilder(voucherLength);

        int numberOfVouchersGenerated = 0;
        int numberOfFailedAttempts = 0;

        while(numberOfVouchersGenerated != numberOfVouchersNeeded)
		{
            voucherBuilder.Clear();
            for (int z = 0; z < voucherLength; z++)
            {
                char symbol = generateRandomSymbol.GetRandomSymbol();
                voucherBuilder.Append(symbol);
            }
            string voucher = voucherBuilder.ToString();

            // Fail attempt - voucher already exist
            if (vouchers.Contains(voucher))
            {
                numberOfFailedAttempts++;
                if (numberOfFailedAttempts == numberOfVouchersNeeded)
                {
                    throw new CannotGenerateUniqueVoucherExceptions(voucherLength: voucherLength, numberOfVouchersNeeded: numberOfVouchersNeeded);
                }
                continue;
            }

            numberOfVouchersGenerated++;
            vouchers.Add(voucher);
        }

		return vouchers;
	}
}

