using System;
namespace VoucherSystem.Exceptions;

public class VoucherUsedException : Exception
{
	public VoucherUsedException(string voucher) : base($"Voucher {voucher} is already used")
	{
	}
}

