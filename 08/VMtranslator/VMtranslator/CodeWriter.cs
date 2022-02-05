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
        private int call_count = 0;

        /// <summary>
        /// コンストラクタ
        /// 出力ファイルを開き書き込む準備を行う
        /// </summary>
        /// <param name="path">出力ファイルパス</param>
        internal CodeWriter(string path)
        {
            sw = new StreamWriter(path);
            line = 0;
            //writeInit();
        }
        /// <summary>
        /// 新しいVMファイルの変換開始を知らせる
        /// </summary>
        internal string FileName { set; get; }
        private Dictionary<string, string> arithmeticAsmDict = new Dictionary<string, string>()
        {
            {"add", "M=M+D" },{"sub","M=M-D"},{"neg","M=-M"},{"and","M=M&D"},{"or","M=M|D"},{"not","M=!M" }
        };
        private Dictionary<string, string> binaryOperatorAsmDict = new Dictionary<string, string>()
        {
             {"add", "M=M+D" },{"sub","M=M-D"},{"and","M=M&D"},{"or","M=M|D"}
        };
        private Dictionary<string, string> binaryConditionAsmDict = new Dictionary<string, string>()
        {
            {"eq", "D;JEQ" },{"gt","D;JGT"},{"lt","D;JLT"}
        };
        private Dictionary<string, string> memoryAsmDict = new Dictionary<string, string>()
        {
            {"argument","ARG"},{"local","LCL"},{"this","THIS"},{"that","THAT" },{ "pointer","3"},{"temp","5"},{"constant","constant"},{"static","static"}
        };

        #region "OK"
        /// <summary>
        /// VMの初期化(ブートストラップ)
        /// </summary>
        internal void writeInit()
        {
            sw.WriteLine($"//Init");
            // VMスタックはRAM[256]から始まる
            sw.WriteLine("@256");
            sw.WriteLine("D=A");
            sw.WriteLine("@SP");
            sw.WriteLine("M=D");
            // 最初に実行を開始するVM関数はSys.init
            writeCall("Sys.init", 0);
        }
        /// <summary>
        /// labelコマンドを行うアセンブリコードに変換し、書き込む
        /// </summary>
        /// <param name="label">ラベル</param>
        internal void writeLabel(string label)
        {
            sw.WriteLine($"//label {label}");
            sw.WriteLine($"({label})");
        }
        /// <summary>
        /// gotoコマンドを行うアセンブリコードに変換し、書き込む
        /// </summary>
        /// <param name="label">ラベル</param>
        internal void writeGoto(string label)
        {
            sw.WriteLine($"//goto {label}");
            sw.WriteLine($"@{label}");
            sw.WriteLine("0;JMP");
        }
        /// <summary>
        /// if-gotoコマンドを行うアセンブリコードに変換し、書き込む
        /// </summary>
        /// <param name="label">ラベル</param>
        internal void writeIf(string label)
        {
            sw.WriteLine($"//if {label}");
            sw.WriteLine(pop_stack_to_D());
            sw.WriteLine($"@${label}");
            sw.WriteLine("D;JNE");
        }
                /// <summary>
        /// callコマンドを行うアセンブリコードに変換し、書き込む
        /// </summary>
        /// <param name="functionName">関数名</param>
        /// <param name="numArgs"></param>
        internal void writeCall(string functionName, int numArgs)
        {
            string labelName = "RETURN_" + (call_count++).ToString();
            sw.WriteLine($"//call {functionName}.{numArgs}");
            sw.WriteLine($"@{labelName}");
            sw.WriteLine("D=A");
            sw.WriteLine(push_D_to_stack());

            writePushPop(typeof(C_PUSH), "local", 0);
            writePushPop(typeof(C_PUSH), "argument", 0);
            writePushPop(typeof(C_PUSH), "this", 0);
            writePushPop(typeof(C_PUSH), "that", 0);

            sw.WriteLine("@SP");
            sw.WriteLine("D=M");
            sw.WriteLine($"@{numArgs + 5}");
            sw.WriteLine("D=D-A");
            sw.WriteLine("@ARG");
            sw.WriteLine("M=D");
            sw.WriteLine("@SP");
            sw.WriteLine("D=M");
            sw.WriteLine("@LCL");
            sw.WriteLine("M=D");
            sw.WriteLine($"@{functionName}");
            sw.WriteLine("0; JMP");
            sw.WriteLine($"({labelName})");
        }

        /// <summary>
        /// functionコマンドを行うアセンブリコードに変換し、書き込む
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="numLocals"></param>
        internal void writeFunction(string functionName, int numLocals)
        {
            sw.WriteLine($"//{functionName}.{numLocals}");
            writeLabel(functionName);
            for (int i = 0; i < numLocals; i++)
            {
                writePushPop(typeof(C_PUSH), "constant", 0);
            }
        }

        /// <summary>
        /// returnコマンドを行うアセンブリコードに変換し、書き込む
        /// </summary>
        internal void writeReturn()
        {
            string FRAME = "R13";
            string RET = "R14";

            sw.WriteLine("//Return");
            // FRAME=LCL
            sw.WriteLine("@LCL");
            sw.WriteLine("D=M");
            sw.WriteLine($"@{FRAME}");
            sw.WriteLine("M=D");
            // RET =* (FRAME - 5)
            sw.WriteLine("@5");
            sw.WriteLine("D=A");
            sw.WriteLine($"@{FRAME}");
            sw.WriteLine("A=M-D");
            sw.WriteLine("D=M");
            sw.WriteLine($"@{RET}");
            sw.WriteLine("M=D");
            // *ARG = pop()
            sw.WriteLine(pop_stack_to_D());
            sw.WriteLine("@ARG");
            sw.WriteLine("A=M");
            sw.WriteLine("M=D");

            // SP = ARG + 1
            sw.WriteLine("@ARG");
            sw.WriteLine("D=M");
            sw.WriteLine("@SP");
            sw.WriteLine("M=D+1");
            // THAT = *(FRAME-1)
            // THIS = *(FRAME-2)
            // ARG = *(FRAME-3)
            // LCL = *(FRAME-4)
            int offset = 1;
            foreach (string s in new string[] { "@THAT", "@THIS", "@ARG", "@LCL" })
            {
                sw.WriteLine($"@{FRAME}");
                sw.WriteLine("D=M");
                sw.WriteLine($"@{offset}");
                sw.WriteLine("A=D-A");
                sw.WriteLine("D=M");
                sw.WriteLine(s);
                sw.WriteLine("M=D");
                offset++;
            }

            // goto RET
            sw.WriteLine($"@{RET}");
            sw.WriteLine("A=M");
            sw.WriteLine("0;JMP");
        }
        #endregion



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
            sw.WriteLine($"//{command}:{segment}:{index}");
            string address = memoryAsmDict[segment];
            if ((segment == "local") |
                (segment == "argument") |
                (segment == "this") |
                (segment == "that"))
            {

                sw.WriteLine($"@{address}");
                sw.WriteLine("D=M");
                if (command == typeof(C_PUSH))
                {
                    sw.WriteLine($"@{index}");
                    sw.WriteLine("A=D+A");
                }
                else
                {
                    sw.WriteLine($"@{index}");
                    sw.WriteLine("D=D+A");
                }
                    
            }
            else if ((segment == "pointer") |
                (segment == "temp"))
            {
                sw.WriteLine($"@R{ int.Parse(address) + index}");
                sw.WriteLine("D=A");
            }
            else if (segment == "constant")
            {
                sw.WriteLine($"@{index}");
            }
            else if (segment == "static")
            {
                sw.WriteLine($"@{ FileName + "." + line}");
                sw.WriteLine("D=M");
                if (command == typeof(C_PUSH))
                {
                    sw.WriteLine($"@{index}");
                    sw.WriteLine("A=D+A");
                }
                else
                {
                    sw.WriteLine($"@{index}");
                    sw.WriteLine("D=D+A");
                }
            }

            if (command == typeof(C_PUSH))
            {
                if (segment == "constant")
                {
                    sw.WriteLine("D=A"); // 定数の代入はAレジスタ
                }
                else
                {
                    sw.WriteLine("D=M");
                }
                sw.WriteLine(push_D_to_stack());
            }
            else if (command == typeof(C_POP))
            {
                sw.WriteLine($"@R13");
                sw.WriteLine("M=D"); // memory set
                sw.WriteLine(decrement_stack_adress());
                sw.WriteLine(set_address_stack());
                sw.WriteLine("D=M");
                sw.WriteLine($"@R13");
                sw.WriteLine("A=M");
                sw.WriteLine("M=D");
            }
            line++;
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
       private string pop_stack_to_D()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(decrement_stack_adress());
            stringBuilder.AppendLine("A=M");
            stringBuilder.AppendLine("D=M");
            return stringBuilder.ToString();
        }

        private string push_D_to_stack()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(set_address_stack());
            stringBuilder.AppendLine("M=D");
            stringBuilder.AppendLine(increment_stack_address());
            return stringBuilder.ToString();
        }
        
        private string set_address_stack()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("@SP");
            stringBuilder.AppendLine("A=M");
            return stringBuilder.ToString();
        }

        internal void close()
        {
            sw.Close();
            sw.Dispose();
        }
    }
}
