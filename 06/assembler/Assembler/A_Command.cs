using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler
{
    internal class A_Command : ICommand
    {
        public A_Command(string param)
        {
            symbol = param.Split('@')[1];
        }

        public string symbol { get; }
        string ICommand.GetName()
        {
            return "A_COMMAND";
        }
    }
}
