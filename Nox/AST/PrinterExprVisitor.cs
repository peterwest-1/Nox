
using System.Globalization;

namespace Nox.AST
{
    internal partial class Printer : Expr.IVisitor<string>
    {
        public string VisitAssignExpr(Expr.Assign expr)
        {
            return ParenthesizeParts("=", expr.name.lexeme, expr.value);
        }

        public string VisitBinaryExpr(Expr.Binary expr)
        {
            return Parenthesize(expr.op.lexeme, expr.left, expr.right);
        }

        public string VisitCallExpr(Expr.Call expr)
        {
            return ParenthesizeParts("call", expr.callee, expr.arguments);
        }

        public string VisitGetExpr(Expr.Get expr)
        {
            return ParenthesizeParts(".", expr.obj, expr.name.lexeme);
        }

        public string VisitGroupingExpr(Expr.Grouping expr)
        {
            return Parenthesize("group", expr.expression);
        }

        public string VisitLiteralExpr(Expr.Literal expr)
        {
            if (expr.value == null) return "nil";
            if (expr.value is double doubleValue)
            {
                return doubleValue % 1 == 0 ? doubleValue.ToString("0.0###", CultureInfo.InvariantCulture) : doubleValue.ToString(CultureInfo.InvariantCulture);
            }
            if (expr.value is bool boolValue)
            {
                return boolValue.ToString().ToLowerInvariant();
            }
            return expr.value.ToString() ?? "LITERAL VALUE NULL";
        }

        public string VisitLogicalExpr(Expr.Logical expr)
        {
            return Parenthesize(expr.op.lexeme, expr.left, expr.right);
        }

        public string VisitSetExpr(Expr.Set expr)
        {
            return ParenthesizeParts("=", expr.obj, expr.name.lexeme, expr.value);
        }

        public string VisitSuperExpr(Expr.Super expr)
        {
            return ParenthesizeParts(Keywords.SUPER, expr.method);
        }

        public string VisitThisExpr(Expr.This expr)
        {
            return Keywords.THIS;
        }

        public string VisitUnaryExpr(Expr.Unary expr)
        {
            return Parenthesize(expr.op.lexeme, expr.right);
        }

        public string VisitVariableExpr(Expr.Variable expr)
        {
            return expr.name.lexeme;
        }
    }
}
