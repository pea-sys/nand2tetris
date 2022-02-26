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
        private VMWriter vw;
        private SymbolTable st;
        private Stack<string> blockStack = new Stack<string>();
        private string className;
        internal CompilationEngine(string in_path,string out_path)
        {
            tokenizer = new JackTokenizer(in_path);
            sw = new StreamWriter(out_path);
            vw = new VMWriter(out_path.Replace(".xml",".vm"));
            st = new SymbolTable();
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
            className = tokenizer.token.raw;
            // className
            compileIdentifier(define:false, string.Empty, IdentifierKindType.NONE); // サブルーチンとクラス名は登録不要
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
            vw.Close();
        }
       
        /// <summary>
        /// ('static'|'field') type varName (',' varName)* ';'
        /// </summary>
        private void compileClassVarDec()
        {
            IdentifierKindType kindType = IdentifierKindType.NONE;
            WriteTagStart("classVarDec");
            // ('static' | 'field')
            if (tokenizer.token.tokenType == TokenType.KEYWORD)
            {
                if (((KeywordToken)tokenizer.token).keywordType == KeywordType.STATIC)
                {
                    kindType = IdentifierKindType.STATIC;
                }
                else if (((KeywordToken)tokenizer.token).keywordType == KeywordType.FIELD)
                {
                    kindType = IdentifierKindType.FIELD;
                }
                else
                {
                    throw new FormatException();
                }
            }
            compileKeyword(KeywordType.STATIC, KeywordType.FIELD);
            // type
            compileType();
            // varName
            compileIdentifier(define:true, className, kindType);

            // (',' varName)* ';'
            bool variableCompile = true;
            while (variableCompile)
            {
                variableCompile = false;
                if (tokenizer.token.raw == ",")
                {
                    compileSymbol(",");
                    compileIdentifier(define: true, tokenizer.token.raw, IdentifierKindType.ARG);
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
            if (tokenizer.token.tokenType == TokenType.KEYWORD)
            {
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
            compileIdentifier(define: true, tokenizer.token.raw,   IdentifierKindType.FIELD);
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
            else
            {
                vw.writePush(SegmentType.CONST, 0);
            }
            compileSymbol(";");
            WriteTagEnd();
            vw.writeReturn();
        }
        /// <summary>
        /// term (op term)*
        /// </summary>
        private void compileExpression()
        {
            string[] ops = new string[] { "+", "-", "*", "/", "&", "|", "<", ">", "=" };
            WriteTagStart("expression");
            // term
            compileTerm();
            // (op term)*
            bool variableCompile = true;
            while (variableCompile)
            {
                variableCompile = false;
                if (ops.Contains(tokenizer.token.raw))
                {
                    string op = tokenizer.token.raw;
                    compileSymbol(tokenizer.token.raw);
                    compileTerm();
                    switch (op)
                    {
                        case "+":
                            vw.writeArithmetic(ArithmeticType.ADD);
                            break;
                        case "-":
                            vw.writeArithmetic(ArithmeticType.SUB);
                            break;
                        case "*":
                            vw.writeCall("Math.multiply", 2);
                            break;
                        case "/":
                            vw.writeCall("Math.divide", 2);
                            break;
                        case "&":
                            vw.writeArithmetic(ArithmeticType.AND);
                            break;
                        case "|":
                            vw.writeArithmetic(ArithmeticType.OR);
                            break;
                        case "<":
                            vw.writeArithmetic(ArithmeticType.GT);
                            break;
                        case ">":
                            vw.writeArithmetic(ArithmeticType.LT);
                            break;
                        case "=":
                            vw.writeArithmetic(ArithmeticType.EQ);
                            break;
                    }
                }
                if (ops.Contains(tokenizer.token.raw))
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
                    vw.writePush(SegmentType.CONST, int.Parse(tokenizer.token.raw));
                    compileIntVal();
                    break;
                case TokenType.stringConstant:

                    vw.writePush(SegmentType.CONST, tokenizer.token.raw.Length);
                    vw.writeCall("String.new", 1);
                    foreach (char c in tokenizer.token.raw)
                    {
                        vw.writePush(SegmentType.CONST, c);
                        vw.writeCall("String.appendChar", 2);
                    }
                
                    compileStringVal();
                    break;
                case TokenType.KEYWORD:
                    switch (((KeywordToken)tokenizer.token).keywordType)
                    {
                        case KeywordType.THIS:
                            vw.writePush(SegmentType.POINTER, 0);
                            break;
                        case KeywordType.TRUE:
                            vw.writePush(SegmentType.CONST, 0);
                            vw.writeArithmetic(ArithmeticType.NOT);
                            break;
                        case KeywordType.FALSE:
                            vw.writePush(SegmentType.CONST, 0);
                            break;
                        case KeywordType.NULL:
                            vw.writePush(SegmentType.CONST, 0);
                            break;
                    }
                    compileKeyword(keywordConstant);
                    break;
                case TokenType.IDENTIFIER:
 
                    if (tokenizer.next_token.raw == "[")
                    {
                        compileIdentifier(define: false, tokenizer.token.raw, IdentifierKindType.NONE);
                        compileSymbol("[");
                        compileExpression();

                        vw.writeArithmetic(ArithmeticType.ADD);
                        vw.writePop(SegmentType.POINTER, 1);
                        vw.writePush(SegmentType.THAT, 0);

                        compileSymbol("]");
                    }
                    // subroutinecall
                    else if (tokenizer.next_token.raw == "(")
                    {
                        compileIdentifier(define: false, tokenizer.token.raw, IdentifierKindType.NONE);
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
                        compileIdentifier(define: false, tokenizer.token.raw, IdentifierKindType.NONE);
                    }
                    break;
                case TokenType.SYMBOL:
                    if (tokenizer.token.raw == "(")
                    {
                        compileSymbol("(");
                        compileExpression();
                        compileSymbol(")");
                    }
                    
                    else if (tokenizer.token.raw == "-")
                    {
                        compileSymbol("-");
                        compileTerm();
                        vw.writeArithmetic(ArithmeticType.NEG);
                    }
                    else if (tokenizer.token.raw == "~")
                    {
                        compileSymbol("~");
                        compileTerm();
                        vw.writeArithmetic(ArithmeticType.NOT);
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
            string instanceName = className; 
            string subroutineName = tokenizer.token.raw;
            int argNum;
            compileIdentifier(define: false, tokenizer.token.raw, IdentifierKindType.NONE);
            if (tokenizer.token.raw == ".")
            {
                compileSymbol(".");
                instanceName = subroutineName;
                subroutineName = tokenizer.token.raw;
                compileIdentifier(define: false, tokenizer.token.raw, IdentifierKindType.NONE);
            }
            compileSymbol("(");
            argNum = compileExpressionList();
            compileSymbol(")");

            vw.writeCall($"{instanceName}.{subroutineName}", argNum);
        }
        /// <summary>
        /// (expression (',' expression) * )?
        /// </summary>
        private int compileExpressionList()
        {
            int argNum = 0;
            WriteTagStart("expressionList");
            if (tokenizer.token.raw != ")")
            {
                bool variableCompile = true;
                while (variableCompile)
                {
                    variableCompile = false;
                    compileExpression();
                    argNum++;
                    if (tokenizer.token.raw == ",")
                    {
                        compileSymbol(",");
                        compileExpression();
                        variableCompile = true;
                        argNum++;
                    }

                }
            }
            WriteTagEnd();
            return argNum;
        }
       
       
        /// <summary>
        /// ('constructor'| 'function' | 'method' ) ( 'void' | type ) subroutineName '(' parameterList ')' subroutineBody
        /// </summary>
        private void compileSubroutineDec()
        {
            st.startSubroutine();
            WriteTagStart("subroutineDec");
            // ('constructor'| 'function' | 'method' )
            KeywordToken token = (KeywordToken)tokenizer.token;
            compileKeyword(KeywordType.CONSTRUCTOR, KeywordType.FUNCTION, KeywordType.METHOD);
            // ('void' | type)
            if (tokenizer.token.raw == "void") { compileKeyword(KeywordType.VOID); }
            else { compileType(); }

            // subroutineName
            string subroutineName = tokenizer.token.raw;
            compileIdentifier(define: true, className, IdentifierKindType.ARG);

            // '(' parameterList ')'
            compileSymbol("(");
            compileParameterList();
            compileSymbol(")");

            //subroutineBody
            compileSubroutineBody(subroutineName, token);

            WriteTagEnd();
        }
        /// <summary>
        /// '{' varDec* statements '}'
        /// </summary>
        private void compileSubroutineBody(string subroutineName, KeywordToken subroutineToken)
        {
            WriteTagStart("subroutineBody");
            compileSymbol("{");
            int local_num = 0;
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
            
            vw.writeFunction($"{className}.{subroutineName}",local_num);

            if (subroutineToken.keywordType == KeywordType.METHOD)
            {
                vw.writePush(SegmentType.ARG, 0);
                vw.writePop(SegmentType.POINTER, 0);
            }
            else if (subroutineToken.keywordType == KeywordType.CONSTRUCTOR)
            {
                vw.writePush(SegmentType.CONST, st.varCount(IdentifierKindType.FIELD));
                vw.writeCall("Memory.alloc", 1);
                vw.writePop(SegmentType.POINTER, 0);
            }
            else if (subroutineToken.keywordType == KeywordType.FUNCTION)
            {

            }
            else
            {
                throw new FormatException();
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
            compileIdentifier(define: true, tokenizer.token.raw, IdentifierKindType.VAR);

            bool variableCompile = true;
            while (variableCompile)
            {
                variableCompile = false;
                if (tokenizer.token.raw == ",")
                {
                    compileSymbol(",");
                    compileIdentifier(define: true, tokenizer.token.raw, IdentifierKindType.VAR);
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
                    compileIdentifier(define: true, tokenizer.token.raw, IdentifierKindType.ARG);
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
                compileIdentifier(define: false, tokenizer.token.raw, IdentifierKindType.NONE);
            }
        }
        private void compileSymbol(params string[] verify)
        {
            SymbolToken token = (SymbolToken)tokenizer.token;
            if (!verify.Contains(token.raw))
            {
                throw new FormatException();
            }
            string values = tokenizer.token.raw;
            values = values.Replace("&", "&amp;");
            values = values.Replace(">", "&gt;");
            values = values.Replace("<", "&lt;");
            Write(GetElement(token.tokenType.ToString().ToLower(), values));
        }
        private void compileIdentifier(bool define,string type, IdentifierKindType kt)
        {
            if (define)
            {
                st.define(tokenizer.token.raw, type, kt);
            }

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
