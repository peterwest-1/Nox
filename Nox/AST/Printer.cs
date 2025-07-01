
using System.Globalization;
using System.Linq.Expressions;
using System.Text;

namespace Nox.AST
{
    internal class Printer : Expr.IVisitor<string>, Stmt.IVisitor<string>
    {
        public string Print(Expr expr)
        {
            return expr.Accept(this);
        }

        public string VisitBinaryExpr(Expr.Binary expr)
        {
            return Parenthesize(expr.op.lexeme,
                       expr.left, expr.right);
        }

        public string VisitExpressionStmt(Stmt.Expression stmt)
        {
            throw new NotImplementedException();
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

        public string VisitPrintStmt(Stmt.Print stmt)
        {
            throw new NotImplementedException();
        }

        public string VisitUnaryExpr(Expr.Unary expr)
        {
            return Parenthesize(expr.op.lexeme, expr.right);
        }

        private string Parenthesize(string name, params Expr[] exprs)
        {
            StringBuilder builder = new();

            builder.Append('(').Append(name);
            foreach (Expr expr in exprs)
            {
                builder.Append(' ');
                builder.Append(expr.Accept(this));
            }
            builder.Append(')');

            return builder.ToString();
        }

        string Expr.IVisitor<string>.VisitAssignExpr(Expr.Assign expr)
        {
            throw new NotImplementedException();
        }

        string Stmt.IVisitor<string>.VisitBlockStmt(Stmt.Block stmt)
        {
            throw new NotImplementedException();
        }

        string Stmt.IVisitor<string>.VisitIfStmt(Stmt.If stmt)
        {
            throw new NotImplementedException();
        }

        string Expr.IVisitor<string>.VisitLogicalExpr(Expr.Logical expr)
        {
            throw new NotImplementedException();
        }

        string Expr.IVisitor<string>.VisitVariableExpr(Expr.Variable expr)
        {
            throw new NotImplementedException();
        }

        string Stmt.IVisitor<string>.VisitVarStmt(Stmt.Var stmt)
        {
            throw new NotImplementedException();
        }

        string Stmt.IVisitor<string>.VisitWhileStmt(Stmt.While stmt)
        {
            throw new NotImplementedException();
        }
    }
}
