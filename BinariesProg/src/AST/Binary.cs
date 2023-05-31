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
    internal class Binary : Expression
    {
        public readonly Expression left;
        public readonly Expression right;
        public readonly Token op;

        public Binary(Expression left, Token op, Expression right, int current) 
        {
            this.left = left;
            this.right = right;
            if (left == null)
            {
                throw new ParseErrorException("Expression at left side of " + op.tokenType.ToString() + " expected.", current);
            }
            if (right == null)
            {
                throw new ParseErrorException("Expression at right side of "+op.tokenType.ToString()+" expected.", current);
            }
            this.op = op;
        }

        public override R Accept<R>(Visitor<R> visitor)
        {
            return visitor.VisitBinaryExpr(this);
        }

        public override BinaryValue Evaluate(int depth)
        {
            //PrintUtils.PrintDepth("We perform an " + op.tokenType.ToString() + " operation on these 2 things: (", depth);
            BinaryValue l = left.Evaluate(depth+1);
            //PrintUtils.PrintDepth("---------------------", depth);
            BinaryValue r = right.Evaluate(depth + 1);
            //PrintUtils.PrintDepth(")", depth);

            switch (op.tokenType)
            {
                case TokenType.AND:
                    {
                        return BinaryUtils.And(l, r);
                    }
                case TokenType.OR:
                    {
                        return BinaryUtils.Or(l, r);
                    }
                case TokenType.XOR:
                    {
                        return BinaryUtils.Xor(l, r);
                    }
                case TokenType.NAND:
                    {
                        return BinaryUtils.Nand(l, r);
                    }
                case TokenType.PLUS:
                    {
                        return l.Add(r);
                    }
                case TokenType.MINUS:
                    {
                        return l.Subtract(r);
                    }
                default:
                    {
                        return l;
                    }
            }


        }
    }
}
