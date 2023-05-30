using System;
namespace VoucherSystem.TestsIntegration.HttpClients
{
	public static class StaticHttpClient
    {
		public static HttpClient shared = new();
    }
}

