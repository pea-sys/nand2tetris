using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMtranslator.Command;

namespace VMtranslator
{
    /// <summary>
    /// VMコマンドをHackアセンブリコードに変換する
    /// </summary>
    internal class CodeWriter
    {
        private StreamWriter sw;
        private int line = 0;
        /// <summary>
        /// コンストラクタ
        /// 出力ファイルを開き書き込む準備を行う
        /// </summary>
        /// <param name="path">出力ファイルパス</param>
        internal CodeWriter(string path)
        {
            sw = new StreamWriter(path);
            line = 0;
        }
        /// <summary>
        /// 新しいVMファイルの変換開始を知らせる
        /// </summary>
        internal string FileName { set; get; }
        /// <summary>
        /// 与えられた算術コマンドをアセンブリコードに変換し、書き込む
        /// </summary>
        /// <param name="command"></param>
        internal void writeArithmetic(string command)
        {
            switch (command)
            {
                case "add":
                    decrement_stack_adress();
                    set_address_stack();
                    sw.WriteLineAsync("D=M");
                    decrement_stack_adress();
                    set_address_stack();
                    sw.WriteLineAsync("M=M+D");
                    break;
                case "sub":
                    sw.WriteLineAsync("M=M-D");
                    break;
                case "neg":
                    sw.WriteLineAsync("-M");
                    break;
                case "and":
                    sw.WriteLineAsync("M=M&D");
                    break;
                case "or":
                    sw.WriteLineAsync("M=M|D");
                    break;
                case "not":
                    sw.WriteLineAsync("!M");
                    break;
               default:
                    sw.WriteLineAsync("D=M-D");
                    sw.WriteLineAsync($"@TRUE_{line}");

                    switch (command)
                    {
                        case "eq":
                            sw.WriteLineAsync("D; JEQ");
                            break;
                        case "gt":
                            sw.WriteLineAsync("D; JGT");
                            break;
                        case "lt":
                            sw.WriteLineAsync("D; JLT");
                            break;
                    }

                    sw.WriteLineAsync("M=0"); // VMはfalseを0で返す
                    sw.WriteLineAsync($"(TRUE_{line})");
                    sw.WriteLineAsync("M=-1"); // VMはtrueを-1で返す
                    break;
            }
            increment_stack_address();
            line++;
        }
        /// <summary>
        /// C_PUSHまたはC_POPコマンドをアセンブリコードに変換し、書き込む
        /// </summary>
        /// <param name="command"></param>
        internal void writePushPop(Type command, string segment, int index)
        {
            string line = string.Empty;
            if (command == typeof(C_PUSH))
            {
                sw.WriteLineAsync($"@{index}");
                if (segment == "constant")
                {
                    sw.WriteLineAsync("D=A"); // 定数の代入はAレジスタ
                }
                else
                {
                    sw.WriteLineAsync("D=M");
                }
                push_stack();
            }
        }
        /// <summary>
        /// スタックポインタを１つ進める
        /// </summary>
        private void increment_stack_address()
        {
            sw.WriteLineAsync("@SP");
            sw.WriteLineAsync("M=M+1");
        }
        /// <summary>
        /// スタックポインタを１つ戻す
        /// </summary>
        private void decrement_stack_adress()
        {
            sw.WriteLineAsync("@SP");
            sw.WriteLineAsync("M=M-1");
        }
        private void set_address_stack()
        {
            sw.WriteLineAsync("@SP");
            sw.WriteLineAsync("A=M");
        }
        private void push_stack()
        {
            sw.WriteLineAsync("@SP");
            sw.WriteLineAsync("A=M");
            sw.WriteLineAsync("M=D");
            increment_stack_address();
        }

        internal void close()
        {
            sw.Close();
            sw.Dispose();
        }
    }
}
