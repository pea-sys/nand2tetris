﻿using System;
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
        public Dictionary<string, string> arithmeticAsmDict = new Dictionary<string, string>()
        {
            {"add", "M=M+D" },{"sub","M=M-D"},{"neg","M=-M"},{"and","M=M&D"},{"or","M=M|D"},{"not","M=!M" }
        };
        public Dictionary<string, string> binaryOperatorAsmDict = new Dictionary<string, string>()
        {
             {"add", "M=M+D" },{"sub","M=M-D"},{"and","M=M&D"},{"or","M=M|D"}
        };
        public Dictionary<string, string> binaryConditionAsmDict = new Dictionary<string, string>()
        {
            {"eq", "D;JEQ" },{"gt","D;JGT"},{"lt","D;JLT"}
        };

        /// <summary>
        /// 与えられた算術コマンドをアセンブリコードに変換し、書き込む
        /// </summary>
        /// <param name="command">コマンド</param>
        internal void writeArithmetic(string command)
        {
            sw.WriteLine($"//{command}");
            sw.WriteLine(decrement_stack_adress());
            sw.WriteLine(set_address_stack());

            if (arithmeticAsmDict.ContainsKey(command))
            {
                if (binaryOperatorAsmDict.ContainsKey(command))
                {
                    sw.WriteLine("D=M");
                    sw.WriteLine(decrement_stack_adress());
                }
                
                sw.WriteLine(set_address_stack());
                sw.WriteLine(arithmeticAsmDict[command]);

            }
            if (binaryConditionAsmDict.ContainsKey(command))
            {
                sw.WriteLine("D=M");
                sw.WriteLine(decrement_stack_adress());
                sw.WriteLine(set_address_stack());
                sw.WriteLine("D=M-D");
                sw.WriteLine($"@TRUE_{line}");
                sw.WriteLine(binaryConditionAsmDict[command]);
                sw.WriteLine($"@FALSE_{line}");
                sw.WriteLine("0;JMP");
                sw.WriteLine($"(TRUE_{line})");
                sw.WriteLine(set_address_stack());
                sw.WriteLine("M=-1"); // VMはtrueを-1で返す
                sw.WriteLine($"@END_{line}");
                sw.WriteLine("0;JMP");
                sw.WriteLine($"(FALSE_{line})");
                sw.WriteLine(set_address_stack());
                sw.WriteLine("M=0"); // VMはfalseを0で返す
                sw.WriteLine($"(END_{line})");
            }

            sw.WriteLine(increment_stack_address());
            line++;
        }
        /// <summary>
        /// C_PUSHまたはC_POPコマンドをアセンブリコードに変換し、書き込む
        /// </summary>
        /// <param name="command">コマンド</param>
        /// <param name="segment">セグメント</param>
        /// <param name="index">インデックス</param>
        internal void writePushPop(Type command, string segment, int index)
        {
            string line = string.Empty;
            if (command == typeof(C_PUSH))
            {
                sw.WriteLine($"@{index}");
                if (segment == "constant")
                {
                    sw.WriteLine("D=A"); // 定数の代入はAレジスタ
                }
                else
                {
                    sw.WriteLine("D=M");
                }
                sw.WriteLine(push_stack());
            }
            sw.WriteLine(increment_stack_address());
        }
        /// <summary>
        /// スタックポインタを１つ進める
        /// </summary>
        private string increment_stack_address()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("@SP");
            stringBuilder.AppendLine("M=M+1");
            return stringBuilder.ToString();
        }
        /// <summary>
        /// スタックポインタを１つ戻す
        /// </summary>
        private string decrement_stack_adress()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("@SP");
            stringBuilder.AppendLine("M=M-1");
            return stringBuilder.ToString();
        }
        private string set_address_stack()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("@SP");
            stringBuilder.AppendLine("A=M");
            return stringBuilder.ToString();
        }
        private string push_stack()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("@SP");
            stringBuilder.AppendLine("A=M");
            stringBuilder.AppendLine("M=D");
            return stringBuilder.ToString();
        }

        internal void close()
        {
            sw.Close();
            sw.Dispose();
        }
    }
}
