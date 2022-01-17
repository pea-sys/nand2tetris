using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler
{
    internal class C_Command : ICommand
    {
        public C_Command(string param)
        {
            string[] split;
            if (param.Contains('='))
            {
                split = param.Split("=");
                dest = split[0];
                comp = split[1];
            }
            else if(param.Contains(';'))
            {
                split = param.Split(";");
                comp = split[0];
                jump = split[1];
            }
        }
        public string dest { get;  }
        public string comp { get; }
        public string jump { get; }
        string ICommand.GetName()
        {
            return "C_COMMAND";
        }
    }
}
