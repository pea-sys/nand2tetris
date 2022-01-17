using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Assembler
{
    class Program
    {

        static void Main(string[] args)
        {
            string inFile;
            string outFile;
            Parser _parser;
            SymbolTable _symbolTable = new SymbolTable();
            int address = 0;
            int digitSymbol = 0;
            
#if DEBUG
            List<string> list = new List<string>();
            args = list.Append(@"C:\Users\user\OneDrive\デスクトップ\コンピュータシステムの理論と実装\nand2tetris\projects\06\pong\pongL.asm").ToArray();
#endif
            if (args.Length != 1)
            {
                throw new ArgumentException("アセンブリファイルを引数に指定してください");
            }
            if (!args[0].Contains(".asm"))
            {
                throw new ArgumentException("アセンブリファイルを引数に指定してください");
            }
            if (!File.Exists(args[0]))
            {
                throw new FileNotFoundException();
            }

            inFile = args[0];
            outFile = args[0].Replace(".asm", ".hack");
            if (File.Exists(outFile))
            {
                File.Delete(outFile);
            }

            //1回目のファイル走査:ROMシンボル辞書作成
            _parser = new Parser(inFile);
            while (_parser.hasMoreCommands)
            {
                _parser.advance();
                if (_parser.commandType == typeof(L_Command))
                {
                    _symbolTable.addEntry(_parser.symbol(), address);
                }
                else
                {
                    address++;
                }
            }

            //2回目のファイル走査:Hackファイル作成
            _parser = new Parser(inFile);
            using (var writer = new StreamWriter(outFile))
            {
                while (_parser.hasMoreCommands)
                {
                    _parser.advance();
                    if (_parser.commandType == typeof(A_Command))
                    {
                        if (int.TryParse(_parser.symbol(), out digitSymbol))
                        {
                            writer.WriteLine(Convert.ToString(digitSymbol, 2).PadLeft(16, '0'));
                        }
                        else
                        {
                            if (!_symbolTable.contains(_parser.symbol()))
                            {
                                // RAMシンボル登録
                                _symbolTable.addEntry(_parser.symbol());
                            }
                            writer.WriteLine(Convert.ToString(_symbolTable.getAddress(_parser.symbol()), 2).PadLeft(16, '0'));
                        }
                    }
                    else if (_parser.commandType == typeof(C_Command))
                    {
                        writer.WriteLine((_parser.comp() + _parser.dest() + _parser.jump()).PadLeft(16, '1'));
                    }
                }
            }
            Console.WriteLine("アセンブリ->機械語変換完了:" + outFile);
            Console.WriteLine("任意のキーを押下してください");
            Console.ReadKey();
        }
    }
}
