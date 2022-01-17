using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler
{
    /// <summary>
    /// HackAssemblyのニーモニックをバイナリコードに変換するクラス
    /// </summary>
    internal class Code
    {
        private readonly Dictionary<string, int> _compDict = new Dictionary<string, int>()
        {
            {"0", 101010 }, {"1", 111111 }, {"-1", 111010}, {"D", 001100 }, {"A", 110000 }, {"!D",1101 },
            {"!A",110001 }, {"-D",1111 },{"-A",110011 },{"D+1",11111 },{"A+1",110111 }, {"D-1",1110 },
            {"A-1",110010 },{"D+A",10 },{"D-A",10011 },{"A-D",111 }, {"D&A",0 },{"D|A",10101 },
            {"M",110000 },{"!M",110001 },{"M+1",110111 },{"M-1",110010 },{"D+M",10 },{"D-M",10011 }, {"M-D",111 },
            {"D&M",0 },{"D|M",10101 }
        };
        private readonly Dictionary<string, int> _destDict = new Dictionary<string, int>()
        {
            {"M", 1 },{"D",10 },{"MD",11},{"A",100},{"AM",101},{"AD",110},{"AMD",111}
        };
        private readonly Dictionary<string, int> _jumpDict = new Dictionary<string, int>()
        {
            {"JGT", 1 },{"JEQ",10 },{"JGE",11},{"JLT",100},{"JNE",101},{"JLE",110},{"JMP",111}
        };

        private readonly string _destByte;
        private readonly string _compByte;
        private readonly string _jumpByte;

        /// <summary>
        /// コンストラクタ
        /// 入力コマンドをバイナリに変換します
        /// </summary>
        /// <param name="command"></param>
        public Code(C_Command command)
        {
            _compByte = GetCompBits(command.comp);
            _destByte = GetDestBits(command.dest);
            _jumpByte = GetJumpBits(command.jump);
        }
        /// <summary>
        /// compニーモニックのバイナリコードを返す
        /// </summary>
        public string comp => _compByte;
        /// <summary>
        /// destニーモニックのバリコードを返す
        /// </summary>
        public string dest => _destByte;
        /// <summary>
        /// jumpニーモニックのバイナリコードを返す
        /// </summary>
        public string jump => _jumpByte;

        /// <summary>
        /// compニーモニックをバイナリコードに変換する
        /// </summary>
        /// <param name="_comp">ニーモニック</param>
        /// <returns>バイナリコード</returns>
        private string GetCompBits(string _comp)
        {
            int bits = 0;
            if (_comp.Contains("M")) bits += 1000000;
            if (_compDict.ContainsKey(_comp))
            {
                bits += _compDict[_comp];
            }

            return bits.ToString("D7");
        }

        /// <summary>
        /// destニーモニックをバイナリコードに変換する
        /// </summary>
        /// <param name="_dest">ニーモニック</param>
        /// <returns>バイナリコード</returns>
        private string GetDestBits(string _dest)
        {
            int bits = 0;
            if (_dest is null)
            {
                return bits.ToString("D3");
            }
            if (_destDict.ContainsKey(_dest))
            {
                bits += _destDict[_dest];
            }
            return bits.ToString("D3");
        }
        /// <summary>
        /// jumpニーモニックをバイナリコードに変換する
        /// </summary>
        /// <param name="_jump">ニーモニック</param>
        /// <returns>バイナリコード</returns>
        private string GetJumpBits(string _jump)
        {
            int bits = 0;
            if (_jump is null)
            {
                return bits.ToString("D3");
            }
            if (_jumpDict.ContainsKey(_jump))
            {
                bits += _jumpDict[_jump];
            }
            return bits.ToString("D3");
        }
    }
}
