using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler
{
    internal enum SegmentType
    {
        [StringAttibute("none")]
        NONE,
        [StringAttibute("constant")]
        CONST,
        [StringAttibute("argument")]
        ARG,
        [StringAttibute("local")]
        LOCAL,
        [StringAttibute("static")]
        STATIC,
        [StringAttibute("that")]
        THAT,
        [StringAttibute("this")]
        THIS,
        [StringAttibute("pointer")]
        POINTER,
        [StringAttibute("temp")]
        TEMP
    }
}
