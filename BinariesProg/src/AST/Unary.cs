using binaries.Conversion;
using binaries.Parsing;
using binaries.Representation;
using binaries.src.Parsing;
using binaries.src.Utils;
using binaries.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace binaries.src.AST
{
    internal class Unary : Expression
    {
        public readonly Token op;
        public readonly Expression right;

        public Unary(Token op, Expression right, int current) 
        { 
            this.op = op;
            this.right = right;
            if (right == null) 
            {
                throw new ParseErrorException("Expression at right side of "+op.tokenType.ToString()+" expected.", current);
            }
        }

        public override R Accept<R>(Visitor<R> visitor)
        {
            return visitor.VisitUnaryExpr(this);
        }

        public override BinaryValue Evaluate(int depth)
        {
            //PrintUtils.PrintDepth("We negate the following thing:(", depth);
            
            if (op.tokenType == TokenType.MINUS)
            {
                return right.Evaluate(depth + 1).Negative2Complement();
            }

            return BinaryUtils.Negate(right.Evaluate(depth + 1));
            //PrintUtils.PrintDepth(")", depth);
        }
    }
}
