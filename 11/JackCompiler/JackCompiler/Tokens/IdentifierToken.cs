using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler.Tokens
{
    internal class IdentifierToken : IToken
    {
        internal IdentifierToken(string value)
        {
            this.raw = value;
            this.tokenType = TokenType.IDENTIFIER;
            this.identifier = value;
        }
        public TokenType tokenType { get; }
        public string identifier { get; }
        public string raw { get; }
    }
}
