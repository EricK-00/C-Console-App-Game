using System;

namespace CSharpConsoleAppGame
{
    enum StatusCondition
    {
        독 = 0b0001,
        화상 = 0b0010,
        마비 = 0b0100,
        얼음 = 0b1000,
        없음 = 0b1_0000
    }
}