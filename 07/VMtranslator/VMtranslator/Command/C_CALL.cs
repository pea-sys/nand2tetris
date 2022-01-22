using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMtranslator.Command
{
    internal class C_CALL : ICommand
    {
        internal C_CALL(string[] param)
        {
            arg1 = param[1];
            arg2 = param[2];
        }

        public string arg1 { get; set; }

        public string arg2 { get; set; }
    }
}
