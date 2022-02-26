using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace JackCompiler
{
    internal class VMWriter
    {
        private StreamWriter sw;
        /// <summary>
        /// 新しいファイルを作り、それに書き込む準備をする
        /// </summary>
        internal VMWriter(string filepath)
        {
            sw = new StreamWriter(filepath);
        }
        /// <summary>
        /// pushコマンドを書く
        /// </summary>
        internal void writePush(SegmentType type, int index)
        {
            sw.WriteLine($"push {type.GetStringValue()} {index}");
        }
        /// <summary>
        /// popコマンドを書く
        /// </summary>
        internal void writePop(SegmentType type, int index)
        {
            sw.WriteLine($"pop {type.GetStringValue()} {index}");
        }
        /// <summary>
        /// 算術コマンドを書く
        /// </summary>
        /// <param name="type"></param>
        internal void writeArithmetic(ArithmeticType type)
        {
            sw.WriteLine(type.GetStringValue());
        }
        /// <summary>
        /// labelコマンドを書く
        /// </summary>
        /// <param name="label"></param>
        internal void writeLabel(string label)
        {
            sw.WriteLine($"label {label}");
        }
        /// <summary>
        /// gotoコマンドを書く
        /// </summary>
        /// <param name="label"></param>
        internal void writeGoto(string label)
        {
            sw.WriteLine($"goto {label}");
        }
        /// <summary>
        /// if-gotoコマンドを書く
        /// </summary>
        /// <param name="label"></param>
        internal void writeIf(string label)
        {
            sw.WriteLine($"if-goto {label}");
        }
        /// <summary>
        /// callコマンドを書く
        /// </summary>
        /// <param name="name"></param>
        /// <param name="nArgs"></param>
        internal void writeCall(string name ,int nArgs)
        {
            sw.WriteLine($"call {name} {nArgs}");
        }
        /// <summary>
        /// functionコマンドを書く
        /// </summary>
        /// <param name="name"></param>
        /// <param name="nLocals"></param>
        internal void writeFunction(string name, int nLocals)
        {
            sw.WriteLine($"function {name} {nLocals}");
        }
        /// <summary>
        /// returnコマンドを書く
        /// </summary>
        internal void writeReturn()
        {
            sw.WriteLine($"return");
        }
        /// <summary>
        /// 出力ファイルを閉じる
        /// </summary>
        internal void Close()
        {
            if (sw != null)
            {
                sw.Flush();
                sw.Close();
            }
            
        }
    }
}
