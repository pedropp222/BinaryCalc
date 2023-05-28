using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace binaries.src.AST
{
    internal interface Visitor<R>
    {
        R VisitBinaryExpr(Binary expr);
        R VisitUnaryExpr(Unary expr);
        R VisitLiteralExpr(Literal expr);
        R VisitGroupExpr(Grouping expr);
    }
}
