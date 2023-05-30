using System;
namespace VoucherSystem.Exceptions;

public class VoucherDoesntExistException : Exception
{
	public VoucherDoesntExistException(string voucher) : base($"Voucher {voucher} doesn't exist")
	{
	}
}

