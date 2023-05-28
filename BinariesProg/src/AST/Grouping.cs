using binaries.Representation;
using binaries.src.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace binaries.src.AST
{
    internal class Grouping : Expression
    {
        public readonly Expression expr;

        public Grouping(Expression expr)
        {
            this.expr = expr;
        }

        public override R Accept<R>(Visitor<R> visitor)
        {
            return visitor.VisitGroupExpr(this);
        }

        public override BinaryValue Evaluate(int depth)
        {
            //PrintUtils.PrintDepth("We evaluate this inner expression: {", depth);
            var b = expr.Evaluate(depth+1);
            //PrintUtils.PrintDepth("}", depth);
            return b;
        }
    }
}
