using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler
{
    internal enum TokenType
    {
        NONE,
        KEYWORD,
        SYMBOL,
        IDENTIFIER,
        integerConstant,
        stringConstant
    }
}
