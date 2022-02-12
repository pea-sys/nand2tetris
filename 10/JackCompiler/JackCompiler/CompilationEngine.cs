using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler
{
    internal class CompilationEngine
    {
        internal CompilationEngine(string in_path,string out_path)
        {

        }
        /// <summary>
        /// クラスをコンパイルする
        /// </summary>
        public void compileClass()
        {

        }
        /// <summary>
        /// スタティック宣言またはフィールド宣言をコンパイルする
        /// </summary>
        public void compileClassVarDec()
        {

        }
        /// <summary>
        /// メソッド、ファンクション、コンストラクタをコンパイルする
        /// </summary>
        public void compileSubroutine()
        { }
        /// <summary>
        /// パラメータのリストをコンパイルする。"()"は含まない
        /// </summary>
        public void compileParameterList()
        { }
        /// <summary>
        /// var宣言をコンパイルする
        /// </summary>
        public void compileVarDec()
        { }
        /// <summary>
        /// 一連の文をコンパイルする。"{}"は含まない
        /// </summary>
        public void compileStatements()
        { }
        /// <summary>
        /// do文をコンパイルする
        /// </summary>
        public void compileDo()
        { }
        /// <summary>
        /// let文をコンパイルする
        /// </summary>
        public void compileLet()
        { }
        /// <summary>
        /// while文をコンパイルする
        /// </summary>
        public void compileWhile()
        { }
        /// <summary>
        /// return文をコンパイルする
        /// </summary>
        public void compileReturn()
        { }
        /// <summary>
        /// if文をコンパイルする。else文を伴う可能性がある
        /// </summary>
        public void compileIf()
        { }
        /// <summary>
        /// 式をコンパイルする
        /// </summary>
        public void compileExpression()
        { }
        /// <summary>
        /// termをコンパイルする
        /// </summary>
        public void compileTerm()
        { }
        /// <summary>
        /// コンマで分離された式のリストをコンパイルする
        /// </summary>
        public void compileExpressionList()
        { }
    }
}
