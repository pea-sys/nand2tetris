using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMtranslator
{
    internal interface ICommand
    {
        string arg1 { get; }
        int arg2 { get; }
    }
}
