using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMtranslator.Command;

namespace VMtranslator
{
    internal class Parser
    {
        int cursor = -1;
        private List<ICommand> commandList = new List<ICommand>();

        /// <summary>
        /// コンストラクタ
        /// 入力ファイルを開きパースを行う準備をする
        /// </summary>
        /// <param name="path">入力ファイルパス</param>
        internal Parser(string path)
        {
            cursor = -1;
            using (StreamReader sr = new StreamReader(path))
            {
                string sentence;
                string[] atom;
                string[] lines = sr.ReadToEnd().Split(new string[]{ Environment.NewLine}, StringSplitOptions.None);

                foreach (string line in lines)
                {
                    sentence = line;
                    //コメント削除
                    int index = sentence.IndexOf("//");
                    if (index != -1)
                    {
                        sentence = sentence.Substring(0, index);
                    }
                    if (sentence.Length < 1)
                    {
                        continue;
                    }
                    atom = sentence.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    switch (atom[0])
                    {
                        case "add":
                        case "sub":
                        case "neg":
                        case "eq":
                        case "qt":
                        case "lt":
                        case "and":
                        case "or":
                        case "not":
                            commandList.Add(new C_ARITHEMETIC(atom));
                            break;
                        case "push":
                            commandList.Add(new C_PUSH(atom));
                            break;
                        case "pop":
                            commandList.Add(new C_POP(atom));
                            break;
                        case "goto":
                            commandList.Add(new C_GOTO());
                            break;
                        case "if-goto":
                            commandList.Add(new C_IF());
                            break;
                        case "function":
                            commandList.Add(new C_FUNCTION(atom));
                            break;
                        case "label":
                            commandList.Add(new C_LABEL());
                            break;
                        case "return":
                            commandList.Add(new C_RETURN());
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// 入力にまだコマンドが存在するか？
        /// </summary>
        internal bool hasMoreCommands
        { get { return (cursor < (commandList.Count() -  1)); } }
        /// <summary>
        /// 入力から次のコマンドを読み、それを現在のコマンドにする
        /// </summary>
        internal void advance()
        {
            cursor++;
            return;
        }
        /// <summary>
        /// 元コマンドの種類を返す
        /// </summary>
        internal Type commandType
        {
            get
            {
                if (!(commandList[cursor] is null))
                {
                    return commandList[cursor].GetType();
                }
                return null;
            }
        }
        internal string arg1
        {
            get
            {
                return commandList[cursor].arg1;
            }
        }
        internal string arg2
        {
            get
            {
                return commandList[cursor].arg2;
            }
        }
    }
}
