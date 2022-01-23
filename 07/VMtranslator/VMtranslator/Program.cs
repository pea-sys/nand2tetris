using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMtranslator.Command;

namespace VMtranslator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Parser _parser;
            CodeWriter _codeWriter;
            IEnumerable<string> _files = new string[] { };
            string outFile = string.Empty;

            // 起動引数チェック
            if (args.Length != 1)
            {
                throw new ArgumentException("vmファイルパスまたはvmファイルが含まれているディレクトリパスを引数に指定してください");
            }
            // ディレクトリチェック
            if (Directory.Exists(args[0]))
            {
                _files = Directory.EnumerateFiles(args[0], "*.vm",SearchOption.AllDirectories);
            }
            // ファイルチェック
            if ((File.Exists(args[0]) & args.Contains("*.vm")))
            {
                _files = new string[] { args[0] };
            }
            if (_files.Count() == 0)
            {
                throw new ArgumentException("vmファイルパスまたはvmファイルが含まれているディレクトリパスを引数に指定してください");
            }

            outFile = Path.ChangeExtension(args[0], ".asm");
            if (File.Exists(outFile))
            {
                File.Delete(outFile);
            }

            _codeWriter = new CodeWriter(Path.ChangeExtension(args[0], ".asm"));

            foreach (string _file in _files)
            {
                _parser = new Parser(_file);
                while (_parser.hasMoreCommands)
                {
                    _parser.advance();
                    if (_parser.commandType != typeof(C_RETURN))
                    {

                    }
                    if ((_parser.commandType == typeof(C_PUSH)) |
                        (_parser.commandType == typeof(C_POP)))
                    {
                        _codeWriter.writePushPop(_parser.commandType, _parser.arg1, int.Parse(_parser.arg2));
                    }
                    else if ((_parser.commandType == typeof(C_ARITHEMETIC)))
                    {
                        _codeWriter.writeArithmetic(_parser.arg1);
                    }
                    else if ((_parser.commandType == typeof(C_FUNCTION)) |
                             (_parser.commandType == typeof(C_CALL)))
                    {

                    }
                }
            }
            _codeWriter.close();
        }
    }
}
