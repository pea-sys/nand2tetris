using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler
{
    internal class L_Command: ICommand
    {
        public L_Command(string param)
        {
            symbol = param.Replace("(","").Replace(")","");
        }

        public string symbol { get; }

        string ICommand.GetName()
        {
            return "L_COMMAND";
        }
    }
}
