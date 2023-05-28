using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace binaries.src.AST
{
    internal class AstPrinter : Visitor<string>
    {
        public string Print(Expression expr)
        {
            return expr.Accept(this);
        }

        private string Parenthesize(string name, List<Expression> exprs)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append('(').Append(name);

            foreach(Expression e in exprs)
            {
                sb.Append(' ');
                if (e != null)
                {
                    sb.Append(e.Accept(this));
                }
                else
                {
                    sb.Append("INVALID");
                }
            }

            sb.Append(')');

            return sb.ToString();
        }

        public string VisitBinaryExpr(Binary expr)
        {
            return Parenthesize(expr.op.value, new List<Expression>() {expr.left,expr.right});
        }

        public string VisitGroupExpr(Grouping expr)
        {
            return Parenthesize("subexp", new List<Expression>() { expr.expr });
        }

        public string VisitLiteralExpr(Literal expr)
        {
            return expr.value;
        }

        public string VisitUnaryExpr(Unary expr)
        {
            return Parenthesize(expr.op.value, new List<Expression>() { expr.right });
        }
    }
}
