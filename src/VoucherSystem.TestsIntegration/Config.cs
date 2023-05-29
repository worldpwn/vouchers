using System;
using Xunit.Abstractions;

namespace VoucherSystem.TestsIntegration;

public static class Config
{
    public static string GetUrlToWebApiFromEnv(ITestOutputHelper output)
    {
        string? urlToWebApiFromEnv = Environment.GetEnvironmentVariable("URL_TO_WEB_API");
        if (urlToWebApiFromEnv is null)
        {
            urlToWebApiFromEnv = "https://localhost:7148";
            output.WriteLine("Cannot find URL_TO_WEB_API enviornment variable");
            output.WriteLine($"Fallback to {urlToWebApiFromEnv}");
        }
        return urlToWebApiFromEnv;
    }
}

