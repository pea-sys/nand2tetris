using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler.Tokens
{
    internal class KeywordToken : IToken
    {
        internal KeywordToken(string value)
        {
            this.raw = value;
            this.tokenType = TokenType.KEYWORD;
            switch (value.ToLower())
            {
                case "class":
                    keywordType = KeywordType.CLASS;
                    break;
                case "method":
                    keywordType = KeywordType.METHOD;
                    break;
                case "function":
                    keywordType = KeywordType.FUNCTION;
                    break;
                case "constructor":
                    keywordType = KeywordType.CONSTRUCTOR;
                    break;
                case "int":
                    keywordType = KeywordType.INT;
                    break;
                case "boolean":
                    keywordType = KeywordType.BOOLEAN;
                    break;
                case "char":
                    keywordType = KeywordType.CHAR;
                    break;
                case "void":
                    keywordType = KeywordType.VOID;
                    break;
                case "var":
                    keywordType = KeywordType.VAR;
                    break;
                case "static":
                    keywordType = KeywordType.STATIC;
                    break;
                case "field":
                    keywordType= KeywordType.FIELD;
                    break;
                case "let":
                    keywordType = KeywordType.LET;
                    break;
                case "do":
                    keywordType = KeywordType.DO;
                    break;
                case "if":
                    keywordType = KeywordType.IF;
                    break;
                case "else":
                    keywordType = KeywordType.ELSE;
                    break;
                case "while":
                    keywordType = KeywordType.WHILE;
                    break;
                case "return":
                    keywordType = KeywordType.RETURN;
                    break;
                case "true":
                    keywordType = KeywordType.TRUE;
                    break;
                case "false":
                    keywordType = KeywordType.FALSE;
                    break;
                case "null":
                    keywordType = KeywordType.NULL;
                    break;
                case "this":
                    keywordType = KeywordType.THIS;
                    break;
            }
        }
        public TokenType tokenType { get; }
        public KeywordType keywordType { get; }

        public string raw { get; }
    }
}
