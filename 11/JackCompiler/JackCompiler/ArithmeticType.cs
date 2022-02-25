using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler
{
    internal enum ArithmeticType
    {
        NONE,
        [StringAttibute("add")]
        ADD,
        [StringAttibute("sub")]
        SUB,
        [StringAttibute("neg")]
        NEG,
        [StringAttibute("eq")]
        EQ,
        [StringAttibute("gt")]
        GT,
        [StringAttibute("lt")]
        LT,
        [StringAttibute("and")]
        AND,
        [StringAttibute("or")]
        OR,
        [StringAttibute("not")]
        NOT
    }
}
