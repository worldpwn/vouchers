using System;
using VoucherSystem.Generator;

namespace VoucherSystem.TestsUnit.Mock;

public class GenerateRandomSymbolMock: IGenerateRandomSymbol
{
    public char desiredChar;
    public GenerateRandomSymbolMock(char desiredChar) => this.desiredChar = desiredChar;
    public char GetRandomSymbol() => desiredChar;
}

