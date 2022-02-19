using JackCompiler.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JackCompiler
{
    /// <summary>
    /// Jack言語のトークン分割
    /// </summary>
    internal class JackTokenizer
    {
        private readonly HashSet<string> keywordSet = new HashSet<string>()
        {
            "class","constructor","function","method","field","static","var","int",
            "char" ,"boolean","void","true","false","null","this","let","do","if","else","while","return"
        };
        private readonly string[] symbolSet = new string[]
        {
            "{","}","(",")","[","]",".",",",";","+","-","*","/","&","|","<",">","=","~"
        };
        int cursor = -1;
        private List<IToken> tokenList = new List<IToken>();
        /// <summary>
        /// コンストラクタ
        /// 入力ファイルを開きパースを行う準備をする
        /// </summary>
        /// <param name="path">入力ファイルパス</param>
        internal JackTokenizer(string path)
        {
            string sentence;
            string[] work_tokens;
            List<string> tokens = new List<string>();

            short integerConstant = 0;
            using (StreamReader sr = new StreamReader(path))
            {
                string[] lines = sr.ReadToEnd().Split(new String[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
               
                bool comment_multiline = false;
                foreach (string line in lines)
                {
                    int comment_endIndex;
                    int comment_startIndex;
                    
                    sentence = line;
                    //ワンライナーコメント削除
                    comment_startIndex = sentence.IndexOf("//");
                    if (comment_startIndex != -1) sentence = sentence.Substring(0, comment_startIndex);
                    //マルチライナーコメント削除
                    comment_endIndex = sentence.IndexOf("*/");
                    comment_startIndex = sentence.IndexOf("/*");
                    // /*
                    if ((comment_startIndex != -1) & (comment_endIndex == -1))
                    {
                        sentence = sentence.Substring(0, comment_startIndex);
                        comment_multiline = true;
                    }
                    // /* */
                    else if ((comment_startIndex != -1) & (comment_endIndex != -1))
                    {
                        sentence = sentence.Substring(0, comment_startIndex);
                    }
                    else if ((comment_startIndex == -1) & (comment_endIndex != -1))
                    {
                        comment_multiline = false;
                        continue;
                    }
                    if (comment_multiline)
                    {
                        continue;
                    }

                    if (sentence.Length < 1) continue;


                    // 空白分割
                    work_tokens = sentence.Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    if (work_tokens.Length < 1) continue;

                    // ""で囲まれているものは結合する
                    string buff = string.Empty;
                    bool buffering = false;
                    foreach ( string token in work_tokens )
                    {
                        if (token[0] == '\"')
                        {
                            buff = token;
                            buffering = true;
                        }
                        else if(buffering)
                        {
                            buff += " " + token;
                            if (token.Contains("\""))
                            {
                                tokens.Add(buff);
                                buffering = false;
                            }
                        }
                        else
                        {
                            tokens.Add(token);
                        }
                    }
                    work_tokens = tokens.ToArray();
                    tokens.Clear();

                    // シンボル分割
                    foreach (string token in work_tokens)
                    {
                        tokens.Add("");
                        for (comment_startIndex = 0; comment_startIndex < token.Length; comment_startIndex++)
                        {

                            if (symbolSet.Contains(token[comment_startIndex].ToString()))
                            {
                                tokens.Add("");
                                tokens[tokens.Count - 1] = tokens[tokens.Count - 1] + token[comment_startIndex].ToString();
                                tokens.Add("");
                            }
                            else
                            {
                                tokens[tokens.Count - 1] = tokens[tokens.Count - 1] + token[comment_startIndex].ToString();
                            }
                        }
                    }
                    tokens.RemoveAll(item => item == "");
                }
                
                // トークンクラスの登録
                foreach (string token in tokens)
                {
                    if (keywordSet.Contains(token))
                    {
                        tokenList.Add(new KeywordToken(token));
                        continue;
                    }
                    else if (symbolSet.Contains(token))
                    {
                        tokenList.Add(new SymbolToken(token));
                    }
                    else if (Int16.TryParse(token, out integerConstant))
                    {
                        tokenList.Add(new IntConstToken(integerConstant));
                    }
                    else if (!Regex.IsMatch(token, @"[^a-zA-z0-9_]") & !char.IsNumber(token[0]))
                    {
                        tokenList.Add(new IdentifierToken(token));
                    }
                    else
                    {
                        tokenList.Add(new StringConstToken(token));
                    }
                }
            }
        }
        /// <summary>
        /// 入力にまだトークンが存在するか？
        /// </summary>
        internal bool hasMoreTokens { get { return (cursor < (tokenList.Count() - 1)); } }
        /// <summary>
        /// 入力から次のトークンを読み、それを現在のトークンにする
        /// </summary>
        internal void advance()
        {
            if (hasMoreTokens)cursor++;
        }
        /// <summary>
        /// 現在のトークンを返す
        /// </summary>
        internal IToken token { get { return tokenList[cursor]; } }
        internal IToken next_token { get { return tokenList[cursor+1]; } }
    }
}
