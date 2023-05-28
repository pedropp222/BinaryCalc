using binaries.Conversion;
using binaries.Parsing;
using binaries.Representation;
using binaries.src.Representation;
using binaries.src.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace binaries.src.AST
{
    internal class Literal : Expression
    {
        public readonly string value;
        public readonly TokenType type;

        public Literal(string value, TokenType type) 
        { 
            this.value = value;
            this.type = type;
        }

        public override R Accept<R>(Visitor<R> visitor)
        {
            return visitor.VisitLiteralExpr(this);
        }

        public override BinaryValue Evaluate(int depth)
        {
            if (type == TokenType.BINARY_VALUE)
            {
                //PrintUtils.PrintDepth("We fetch the binary value " + value, depth);
                return new BinaryValue(value);
            }
            else if (type == TokenType.DECIMAL_VALUE)
            {
                return new BinaryValue(BinaryConverter.DecimalToBinary(int.Parse(value)).value,false,true);
            }
            else
            {
                return BinaryConverter.HexadecimalToBinary(value);
            }
        }
    }
}
