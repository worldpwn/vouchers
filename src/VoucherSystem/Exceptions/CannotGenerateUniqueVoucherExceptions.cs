using System;
namespace VoucherSystem.Exceptions;

public class CannotGenerateUniqueVoucherExceptions : Exception
{
	public CannotGenerateUniqueVoucherExceptions(int voucherLength, int numberOfVouchersNeeded) :
		base($"Cannot Generate Unique Vouchers where Voucher Length is {voucherLength} and Number Of Vouchers Needed is {numberOfVouchersNeeded}. " +
			 $"Because each Voucher have to be unique try to increase Voucher Length.")
	{

	}
}

