using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMtranslator.Command
{
    internal class C_ARITHEMETIC : ICommand
    {
        internal C_ARITHEMETIC(string[] param)
        {
            arg1 = param[0];
        }
        public string arg1 { get; set; }

    public int arg2 => throw new NotImplementedException();
    }
}
