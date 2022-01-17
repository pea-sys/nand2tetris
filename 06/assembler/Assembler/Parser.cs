using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler
{ 
    /// <summary>
    /// 各アセンブリコマンドをその基本要素に分解するクラス
    /// </summary>
    internal class Parser
    {
        private StreamReader sr;
        private ICommand _currentCommand;
        private Code _currentCode;
        string[] lines;
        int cursor = -1;

        private List<ICommand> commandList = new List<ICommand> ();
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
                string pline;
                lines = sr.ReadToEnd().Split(Environment.NewLine);

                foreach (string line in lines)
                {
                    // 空白文字削除
                    pline = line.Replace(" ","");
                    //コメント削除
                    int index = pline.IndexOf("//");
                    if (index != -1)
                    {
                        pline = pline.Substring(0, index);
                    }
                    if (pline.Length < 1)
                    {
                        continue;
                    }
                    if (pline.StartsWith('@'))
                    {
                        _currentCommand = new A_Command(pline);
                        commandList.Add(_currentCommand);
                    }
                    else if (pline.StartsWith('('))
                    {
                        _currentCommand = new L_Command(pline);
                        commandList.Add(_currentCommand);
                    }
                    else if (pline.Contains('=') | line.Contains(';'))
                    {
                        _currentCommand = new C_Command(pline);
                        commandList.Add(_currentCommand);
                    }
                }
            }
        }
        /// <summary>
        /// 入力にまだコマンドが存在するか？
        /// </summary>
        internal bool hasMoreCommands
        { get { return (cursor < commandList.Count - 1);  } }

        /// <summary>
        /// 入力から次のコマンドを読み、それを現在のコマンドにする
        /// </summary>
        internal void advance()
        {
            cursor++;
            _currentCommand = commandList[cursor];
            if (_currentCommand is C_Command)
            {
                _currentCode = new Code((C_Command)_currentCommand);
            }
            return;
        }
        
        /// <summary>
        /// 元コマンドの種類を返す
        /// </summary>
        internal Type commandType 
        {
            get
            {
                if (_currentCommand is not null)
                {
                    return _currentCommand.GetType();
                }
                return null;
            }
        }
        /// <summary>
        /// 現コマンド@Xxxまたは(Xxx)のXxxを返す
        /// </summary>
        /// <returns>シンボルまたは10進数</returns>
        internal string symbol()
        {
            if (_currentCommand is A_Command)
            {
                return ((A_Command)_currentCommand).symbol;
            }
            else if (_currentCommand is L_Command)
            {
                return ((L_Command)_currentCommand).symbol;
            }
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 現C命令のdestニーモニックを返す
        /// </summary>
        /// <returns>現C命令のdestニーモニック</returns>
        internal string dest()
        {
            if (_currentCommand is C_Command)
            {
                return _currentCode.dest;
            }
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 現C命令のcompニーモニックを返す
        /// </summary>
        /// <returns>現C命令のcompニーモニック</returns>
        /// <exception cref="InvalidOperationException"></exception>
        internal string comp()
        {
            if (_currentCommand is C_Command)
            {
                return _currentCode.comp;
            }
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 現C命令のjumpニーモニックを返す
        /// </summary>
        /// <returns>現C命令のjumpニーモニック</returns>
        internal string jump()
        {
            if (_currentCommand is C_Command)
            {
                return _currentCode.jump;
            }
            throw new InvalidOperationException();
        }
    }
}
