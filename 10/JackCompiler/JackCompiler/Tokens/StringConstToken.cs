using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler.Tokens
{
    internal class StringConstToken : IToken
    {
        internal StringConstToken(string value)
        {
            this.raw = value;
            this.tokenType = TokenType.STRING_CONST;
            this.stringVal = value;
        }
        public TokenType tokenType { get; }
        public string stringVal { get; }
        public string raw { get; }
    }
}
