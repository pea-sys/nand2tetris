using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<string> _files = new string[] { };
            JackTokenizer _jackTokenizer;
            string out_file = string.Empty;
            // 起動引数チェック
            if (args.Length != 1)
            {
                throw new ArgumentException("vmファイルパスまたはvmファイルが含まれているディレクトリパスを引数に指定してください");
            }
            // ディレクトリチェック
            if (Directory.Exists(args[0]))
            {
                _files = Directory.EnumerateFiles(args[0], "*.jack", SearchOption.AllDirectories);
            }
            // ファイルチェック
            if ((File.Exists(args[0]) & args.Contains("*.jack")))
            {
                _files = new string[] { args[0] };
            }
            if (_files.Count() == 0)
            {
                throw new ArgumentException("vmファイルパスまたはvmファイルが含まれているディレクトリパスを引数に指定してください");
            }
            foreach (string _file in _files)
            {
                _jackTokenizer = new JackTokenizer(_file);
                out_file = _file.Replace(".jack", ".xml");
                Debug.Print(out_file);
            }
        }
    }
}
