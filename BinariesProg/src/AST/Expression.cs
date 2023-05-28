using binaries.Representation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace binaries.src.AST
{
    internal abstract class Expression
    {
        public abstract R Accept<R>(Visitor<R> visitor);
        public abstract BinaryValue Evaluate(int depth = 0);
    }
}
