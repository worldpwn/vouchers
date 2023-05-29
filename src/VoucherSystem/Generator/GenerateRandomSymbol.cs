using System;
using static System.Net.Mime.MediaTypeNames;

namespace VoucherSystem.Generator;

public interface IGenerateRandomSymbol
{
	char GetRandomSymbol();
}

public class GenerateRandomSymbol : IGenerateRandomSymbol
{
    const string AVAILABLE_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public char GetRandomSymbol()
    {
        Random random = Random.Shared;
        int index = random.Next(AVAILABLE_CHARS.Length);
        return AVAILABLE_CHARS[index];
    }
}

