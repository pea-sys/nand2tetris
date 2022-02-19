using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JackCompiler.Tokens;
namespace JackCompiler
{
    internal class CompilationEngine:IDisposable
    {
        private JackTokenizer tokenizer;
        private StreamWriter sw;
        private Stack<string> blockStack = new Stack<string>();
        internal CompilationEngine(string in_path,string out_path)
        {
            tokenizer = new JackTokenizer(in_path);
            sw = new StreamWriter(out_path);
            
        }
        public void compileKeyword(params KeywordType[] verify)
        {
            KeywordToken token = (KeywordToken)tokenizer.token;
            if (!verify.Contains(token.keywordType))
            {
                throw new FormatException();
            }
            Write(GetElement(token.tokenType.ToString().ToLower(), token.keywordType.ToString()).ToLower());
        }
        /// <summary>
        /// 'class' className '{' classVarDec* subroutineDec* '}'
        /// </summary>
        public void compileClass()
        {
            tokenizer.advance();
            WriteTagStart("class");
            // class
            compileKeyword(KeywordType.CLASS);
            // className
            compileIdentifier();
            // '{'
            compileSymbol("{");

            bool variableCompile = true;
            while (variableCompile)
            {
                // 'classVarDec*  subroutineDec*
                if (tokenizer.token.tokenType == TokenType.KEYWORD)
                {
                    KeywordToken token = (KeywordToken)tokenizer.token;
                    if ((token.keywordType == KeywordType.STATIC) |
                        (token.keywordType == KeywordType.FIELD))
                    {
                        compileClassVarDec();
                    }
                    else if ((token.keywordType == KeywordType.CONSTRUCTOR) |
                            (token.keywordType == KeywordType.FUNCTION) |
                            (token.keywordType == KeywordType.METHOD))
                    {
                        compileSubroutineDec();
                    }
                    else
                    {
                        variableCompile = false;
                    }
                }
                else
                {
                    variableCompile = false;
                }
            }
            compileSymbol("}");
            WriteTagEnd();
            sw.Flush();
            sw.Close();
        }
       
        /// <summary>
        /// ('static'|'field') type varName (',' varName)* ';'
        /// </summary>
        private void compileClassVarDec()
        {
            WriteTagStart("classVarDec");
            // ('static' | 'field')
            compileKeyword(KeywordType.STATIC, KeywordType.FIELD);
            // type
            compileType();
            // varName
            compileIdentifier();

            // (',' varName)* ';'
            bool variableCompile = true;
            while (variableCompile)
            {
                variableCompile = false;
                if (tokenizer.token.raw == ",")
                {
                    compileSymbol(",");
                    compileIdentifier();
                    variableCompile = true;
                }
            }
            compileSymbol(";");
            WriteTagEnd();
        }

        /// <summary>
        /// (letStatement | ifStatement | whileStatement | doStatement | returnStatement)*
        /// </summary>
        private void compileStatements()
        {
            WriteTagStart("statements");
            bool variableCompile = true;
            while (variableCompile)
            {
                variableCompile = false;

                KeywordToken token = (KeywordToken)tokenizer.token;
                switch (token.keywordType)
                {
                    case KeywordType.LET:
                        compileLetStatement();
                        break;
                    case KeywordType.IF:
                        compileIfStatement();
                        break;
                    case KeywordType.WHILE:
                        compileWhileStatement();
                        break;
                    case KeywordType.DO:
                        compileDoStatement();
                        break;
                    case KeywordType.RETURN:
                        compileRetuenStatement();
                        break;
                }
                if (tokenizer.token.tokenType == TokenType.KEYWORD)
                {
                    token = (KeywordToken)tokenizer.token;
                    if ((token.keywordType == KeywordType.LET) | 
                        (token.keywordType == KeywordType.IF) | 
                        (token.keywordType == KeywordType.WHILE) |
                        (token.keywordType == KeywordType.DO) |
                        (token.keywordType == KeywordType.RETURN))
                    {
                        variableCompile = true;
                    }
                }
            }
            WriteTagEnd();
        }
        /// <summary>
        /// 'let' varName ('[' expression ']')? '=' expression ';'
        /// </summary>
        private void compileLetStatement()
        {
            WriteTagStart("letStatement");
            compileKeyword(KeywordType.LET);
            compileIdentifier();
            if (tokenizer.token.raw == "[")
            {
                compileSymbol("[");
                compileExpression();
                compileSymbol("]");
            }
            compileSymbol("=");
            compileExpression();
            compileSymbol(";");
            WriteTagEnd();
        }
        /// <summary>
        /// 'if' '(' expression ')' '{' statements '}' ('else' '{' statements '}')?
        /// </summary>
        private void compileIfStatement()
        {
            WriteTagStart("ifStatement");
            compileKeyword(KeywordType.IF);
            compileSymbol("(");
            compileExpression();
            compileSymbol(")");
            compileSymbol("{");
            compileStatements();
            compileSymbol("}");
            
            if (tokenizer.token.raw == "else")
            {
                compileKeyword(KeywordType.ELSE);
                compileSymbol("{");
                compileStatements();
                compileSymbol("}");
            }
            WriteTagEnd();
        }
        /// <summary>
        /// 'while' '(' expression ')' '{' statements '}'
        /// </summary>
        private void compileWhileStatement()
        {
            WriteTagStart("whileStatement");
            compileKeyword(KeywordType.WHILE);
            compileSymbol("(");
            compileExpression();
            compileSymbol(")");
            compileSymbol("{");
            compileStatements();
            compileSymbol("}");
            WriteTagEnd();
        }
        /// <summary>
        /// 'do' subroutineCall ';'
        /// </summary>
        private void compileDoStatement()
        {
            WriteTagStart("doStatement");
            compileKeyword(KeywordType.DO);
            compileSubroutineCall();
            compileSymbol(";");
            WriteTagEnd();
        }
        /// <summary>
        /// 'return' expression? ';'
        /// </summary>
        private void compileRetuenStatement()
        {
            WriteTagStart("returnStatement");
            compileKeyword(KeywordType.RETURN);
            if ( tokenizer.token.raw != ";")
            {
                compileExpression();
            }
            compileSymbol(";");
            WriteTagEnd();
        }
        /// <summary>
        /// term (op term)*
        /// </summary>
        private void compileExpression()
        {
            string[] op = new string[] { "+", "-", "*", "/", "&", "|", "<", ">", "=" };
            WriteTagStart("expression");
            // term
            compileTerm();
            // (op term)*
            bool variableCompile = true;
            while (variableCompile)
            {
                variableCompile = false;
                if (op.Contains(tokenizer.token.raw))
                {
                    string values = tokenizer.token.raw;
                    values = values.Replace(">", "&gt;");
                    values = values.Replace("&", "&amp;");
                    values = values.Replace("<", "&lt;");
                    compileSymbol(tokenizer.token.raw);
                    compileTerm();
                }
                if (op.Contains(tokenizer.token.raw))
                {
                    variableCompile = true;
                }
            }
            WriteTagEnd();
        }
        /// <summary>
        /// term
        /// integeConstant | stringConstant | keywordConstant | varName | varName '[' expression ']' |
        /// subroutineCall | '(' expression ')' | unaryOp term
        /// </summary>
        private void compileTerm()
        {
            KeywordType[] keywordConstant = new KeywordType[] { KeywordType.TRUE,KeywordType.FALSE,KeywordType.NULL,KeywordType.THIS  };
            WriteTagStart("term");
            // term
            switch (tokenizer.token.tokenType)
            {
                case TokenType.integerConstant:
                    compileIntVal();
                    break;
                case TokenType.stringConstant:
                    compileStringVal();
                    break;
                case TokenType.KEYWORD:
                    compileKeyword(keywordConstant);
                    break;
                case TokenType.IDENTIFIER:
 
                    if (tokenizer.next_token.raw == "[")
                    {
                        compileIdentifier();
                        compileSymbol("[");
                        compileExpression();
                        compileSymbol("]");
                    }
                    // subroutinecall
                    else if (tokenizer.next_token.raw == "(")
                    {
                        compileIdentifier();
                        compileSymbol("(");
                        compileExpressionList();
                        compileSymbol(")");
                    }
                    else if (tokenizer.next_token.raw == ".")
                    {
                        compileSubroutineCall();
                    }
                    else
                    {
                        compileIdentifier();
                    }
                    break;
                case TokenType.SYMBOL:
                    if (tokenizer.token.raw == "(")
                    {
                        compileSymbol("(");
                        compileExpression();
                        compileSymbol(")");
                    }
                    
                    else if ((tokenizer.token.raw == "-") | (tokenizer.token.raw == "~"))
                    {
                        compileSymbol("-", "~");
                        compileTerm();
                    }
                    break;
            }
            WriteTagEnd();
        }
        /// <summary>
        /// subroutineName '(' expressionList ')' | ( className | varName) '.' subroutineName '(' expressionList ')'
        /// </summary>
        private void compileSubroutineCall()
        {
            compileIdentifier();
            if (tokenizer.token.raw == ".")
            {
                compileSymbol(".");
                compileIdentifier();
            }
            compileSymbol("(");
            compileExpressionList();
            compileSymbol(")");
        }
        /// <summary>
        /// (expression (',' expression) * )?
        /// </summary>
        private void compileExpressionList()
        {
            WriteTagStart("expressionList");
            if (tokenizer.token.raw != ")")
            {
                bool variableCompile = true;
                while (variableCompile)
                {
                    variableCompile = false;
                    compileExpression();
                    if (tokenizer.token.raw == ",")
                    {
                        compileSymbol(",");
                        compileExpression();
                        variableCompile = true;
                    }

                }
            }
            WriteTagEnd();
        }
       
       
        /// <summary>
        /// ('constructor'| 'function' | 'method' ) ( 'void' | type ) subroutineName '(' parameterList ')' subroutineBody
        /// </summary>
        private void compileSubroutineDec()
        {
            WriteTagStart("subroutineDec");
            // ('constructor'| 'function' | 'method' )
            compileKeyword(KeywordType.CONSTRUCTOR, KeywordType.FUNCTION, KeywordType.METHOD);
            // ('void' | type)
            if (tokenizer.token.raw == "void") { compileKeyword(KeywordType.VOID); }
            else { compileType(); }

            // subroutineName
            compileIdentifier();

            // '(' parameterList ')'
            compileSymbol("(");
            compileParameterList();
            compileSymbol(")");

            //subroutineBody
            compileSubroutineBody();

            WriteTagEnd();
        }
        /// <summary>
        /// '{' varDec* statements '}'
        /// </summary>
        private void compileSubroutineBody()
        {
            WriteTagStart("subroutineBody");
            compileSymbol("{");

            while (true)
            {
                if (((KeywordToken)tokenizer.token).keywordType == KeywordType.VAR)
                {
                    compileVarDec();
                }
                else
                {
                    break;
                }
            }

           

            if (tokenizer.token.raw != "}")
            {
                compileStatements();
            }
            

            compileSymbol("}");
            WriteTagEnd();
        }
        /// <summary>
        /// 'var' type varName (',' varName )* ';'
        /// </summary>
        private void compileVarDec()
        {
            WriteTagStart("varDec");
            compileKeyword(KeywordType.VAR);
            compileType();
            compileIdentifier();

            bool variableCompile = true;
            while (variableCompile)
            {
                variableCompile = false;
                if (tokenizer.token.raw == ",")
                {
                    compileSymbol(",");
                    compileIdentifier();
                }
            }
            compileSymbol(";");
            WriteTagEnd();
        }
        private void compileParameterList()
        {
            WriteTagStart("parameterList");
            bool variableCompile = true;
            bool first = true;
            while (variableCompile)
            {
                variableCompile = false;
                if (tokenizer.token.raw != ")")
                {
                    if (!first)
                    {
                        compileSymbol(",");
                    }
                    compileType();
                    compileIdentifier();
                    variableCompile = true;
                    first = false;
                }
            }
            WriteTagEnd();
        }
        private void compileType()
        {
            if (tokenizer.token.tokenType == TokenType.KEYWORD)
            {
                compileKeyword(KeywordType.INT, KeywordType.CHAR, KeywordType.BOOLEAN);
            }
            else
            {
                compileIdentifier();
            }
        }
        private void compileSymbol(params string[] verify)
        {
            SymbolToken token = (SymbolToken)tokenizer.token;
            if (!verify.Contains(token.raw))
            {
                throw new FormatException();
            }
            Write(GetElement(token.tokenType.ToString().ToLower(), token.symbol));
        }
        private void compileIdentifier()
        {
            Write(GetElement(tokenizer.token.tokenType.ToString().ToLower(),
                ((IdentifierToken)tokenizer.token).identifier));
        }
        private void compileIntVal()
        {
            Write(GetElement(tokenizer.token.tokenType.ToString(),
                ((IntConstToken)tokenizer.token).intVal.ToString()));
        }
        private void compileStringVal()
        {
            Write(GetElement(tokenizer.token.tokenType.ToString(),
                ((StringConstToken)tokenizer.token).stringVal));
        }

        private string GetElement(string element, string value)
        {
            string indent = new string(' ', (blockStack.Count()*2) + 1);
            return $"{indent}<{element}> {value} </{element}>";
        }
        private void WriteTagStart(string element)
        {
            string indent = new string(' ', (blockStack.Count()*2) + 1);
            blockStack.Push(element);
            string values = $"{indent}<{element}>";
            sw.WriteLine(values);
            System.Diagnostics.Debug.WriteLine(values);
        }
        private void WriteTagEnd()
        {
            string end = blockStack.Pop();
            string indent = new string(' ', blockStack.Count());
            string values = $"{indent}</{end}>";
            sw.WriteLine(values);
            System.Diagnostics.Debug.WriteLine(values);
        }
        private void Write(string values)
        {
            sw.WriteLine(values);
            System.Diagnostics.Debug.WriteLine(values);
            tokenizer.advance();
        }
        public void Dispose()
        {
            sw.Dispose();
        }
    }
}
