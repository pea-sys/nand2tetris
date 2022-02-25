using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler
{
    internal class SymbolTable
    {
        private Dictionary<string,Identifier> staticDict;
        private Dictionary<string, Identifier> fieldDict;
        private Dictionary<string, Identifier> argumentDict;
        private Dictionary<string, Identifier> varDict;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal SymbolTable()
        {
            staticDict = new Dictionary<string, Identifier>();
            fieldDict = new Dictionary<string, Identifier>();
            argumentDict = new Dictionary<string, Identifier>();
            varDict = new Dictionary<string, Identifier>();
        }
        /// <summary>
        /// 新しいサブルーチンのスコープを開始する
        /// </summary>
        internal void startSubroutine()
        {
            argumentDict.Clear();
            varDict.Clear();
        }
        /// <summary>
        /// 引数の名前、型、属性で指定された新しい識別子を定義し、それに実行インデックスを割り当てる。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="k"></param>
        internal void define(string name, string type, IdentifierKindType k)
        {
            switch (k)
            {
                case IdentifierKindType.ARG:
                    argumentDict[name] = new Identifier(k, type, varCount(k));
                    break;
                case IdentifierKindType.STATIC:
                    staticDict[name] = new Identifier(k, type, varCount(k));
                    break;
                case IdentifierKindType.VAR:
                    varDict[name] = new Identifier(k, type, varCount(k));
                    break;
                case IdentifierKindType.FIELD:
                    fieldDict[name] = new Identifier(k, type, varCount(k));
                    break;
            }
        }
        /// <summary>
        /// 引数で与えられた属性について、それが現在のスコープで定義されている数を返す
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        internal int varCount(IdentifierKindType k)
        {
            switch (k)
            {
                case IdentifierKindType.ARG:
                    return argumentDict.Count();
                case IdentifierKindType.STATIC:
                    return staticDict.Count();
                case IdentifierKindType.FIELD:
                    return fieldDict.Count();
                case IdentifierKindType.VAR:
                    return varDict.Count();
                default:
                    return 0;
            }
        }
        /// <summary>
        /// 引数で与えられた名前の識別子を現在のスコープで探し、その属性を返す。
        /// 見つからなければNONEで返す。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal IdentifierKindType kindOf(string name)
        {
            if (argumentDict.ContainsKey(name)) return argumentDict[name].KindType;
            if (staticDict.ContainsKey(name)) return staticDict[name].KindType;
            if (fieldDict.ContainsKey(name)) return fieldDict[name].KindType;
            if (varDict.ContainsKey(name)) return varDict[name].KindType;
            return IdentifierKindType.NONE;
        }
        /// <summary>
        /// 引数で与えられた名前の識別子現在のスコープで探し、その型を返す
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal string typeOf(string name)
        {
            if (argumentDict.ContainsKey(name)) return argumentDict[name].Kind;
            if (staticDict.ContainsKey(name)) return staticDict[name].Kind;
            if (fieldDict.ContainsKey(name)) return fieldDict[name].Kind;
            if (varDict.ContainsKey(name)) return varDict[name].Kind;
            return String.Empty;
        }

        /// <summary>
        /// 引数で与えられた名前の識別子を現在のスコープで探し、そのインデックスを返す
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal int indexOf(string name)
        {
            if (argumentDict.ContainsKey(name)) return argumentDict[name].Index;
            if (staticDict.ContainsKey(name))   return staticDict[name].Index;
            if (fieldDict.ContainsKey(name)) return fieldDict[name].Index;
            if (varDict.ContainsKey(name)) return varDict[name].Index;
            return -1;
        }
    }
}
