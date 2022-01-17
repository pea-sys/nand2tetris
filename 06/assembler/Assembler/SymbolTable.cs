using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler
{
    /// <summary>
    /// シンボルテーブルクラス
    /// </summary>
    internal class SymbolTable
    {
        private Dictionary<string, int> _symbols;
        private int _ram_memory_count = 16;

        public SymbolTable()
        {
			_symbols = new Dictionary<string, int>()
			{
				{"SP", 0 },{"LCL", 1 },{"ARG", 2},{"THIS", 3},{"THAT", 4 },
				{"R0", 0 },{"R1", 1 },{"R2",2},{"R3",3},{"R4",4},{"R5",5},
                {"R6", 6 },{"R7", 7 },{"R8",8},{"R9",9},{"R10",10},{"R11",11},
                {"R12", 12 },{"R13", 13 },{"R14",14},{"R15",15},
                {"SCREEN", 16384 },{"KBD", 24576}
            };
		}
        public void addEntry(string symbol, int address=-1)
        {
            // ROMシンボル登録
            if (address != -1)
            {
                _symbols.Add(symbol, address);
            }
            // RAMシンボル登録
            else
            {
                _symbols.Add(symbol, _ram_memory_count);
                _ram_memory_count++;
            }
        }
        public bool contains(string symbol)
        { return _symbols.ContainsKey(symbol); }

        public int getAddress(string symbol)
        { return _symbols[symbol]; }
    }
}
