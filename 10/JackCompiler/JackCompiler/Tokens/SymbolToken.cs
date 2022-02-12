using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler.Tokens
{
    internal class SymbolToken : IToken
    {
        internal SymbolToken(string value)
        {
            this.raw = value;
            this.tokenType = TokenType.SYMBOL;
            this.symbol = value;
        }
        public TokenType tokenType { get; }
        public string symbol { get; }
        public string raw { get; }
    }
}
