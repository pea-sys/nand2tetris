using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMtranslator.Command
{
    internal class C_LABEL : ICommand
    {

        internal C_LABEL(string[] param)
        {
            arg1 = param[1];
        }
        public string arg1 { get; set; }

        public int arg2 => throw new NotImplementedException();
    }
}
