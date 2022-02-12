using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler.Tokens
{
    internal class IntConstToken : IToken
    {
        internal IntConstToken(short value)
        {
            this.raw = value.ToString();
            this.tokenType = TokenType.INT_CONST;
            this.intVal = value;
        }
        public TokenType tokenType { get; }
        public short intVal { get; }
        public string raw { get; } 
    }
}
