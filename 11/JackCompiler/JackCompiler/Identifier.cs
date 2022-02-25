using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler
{
    internal class Identifier
    {
        private IdentifierKindType _kindType = IdentifierKindType.NONE;
        private string _kind;
        private int _index;

        internal Identifier(IdentifierKindType _kindType, string _kind, int _index)
        {
            this._kindType = _kindType;
            this._kind = _kind;
            this._index = _index;
        }

        public IdentifierKindType KindType { get { return this._kindType; } }
        public string Kind { get { return this._kind; } }
        public int Index { get { return this._index; } }
    }
}
