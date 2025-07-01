

using System.Globalization;
using System.Text;

namespace Nox.AST
{
    internal class Printer : Expr.IVisitor<string>, Stmt.IVisitor<string>
    {
        public string Print(Expr expr)
        {
            return expr.Accept(this);
        }

        public string Print(Stmt stmt)
        {
            return stmt.Accept(this);
        }

        public string VisitBinaryExpr(Expr.Binary expr)
        {
            return Parenthesize(expr.op.lexeme,
                       expr.left, expr.right);
        }

        public string VisitExpressionStmt(Stmt.Expression stmt)
        {
            return Parenthesize(";", stmt.expression);
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
            return Parenthesize("print", stmt.expression);
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
            return ParenthesizeParts("=", expr.name.lexeme, expr.value);
        }

        string Stmt.IVisitor<string>.VisitBlockStmt(Stmt.Block stmt)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("(block ");

            foreach (Stmt statement in stmt.statements)
            {
                builder.Append(statement.Accept(this));
            }

            builder.Append(")");
            return builder.ToString();
        }

        string Stmt.IVisitor<string>.VisitIfStmt(Stmt.If stmt)
        {
            if (stmt.elseBranch == null)
            {
                return ParenthesizeParts("if", stmt.condition, stmt.thenBranch);
            }

            return ParenthesizeParts("if-else", stmt.condition, stmt.thenBranch,
                stmt.elseBranch);
        }

        string Expr.IVisitor<string>.VisitLogicalExpr(Expr.Logical expr)
        {
            return Parenthesize(expr.op.lexeme, expr.left, expr.right);
        }

        string Expr.IVisitor<string>.VisitVariableExpr(Expr.Variable expr)
        {
            return expr.name.lexeme;
        }

        string Stmt.IVisitor<string>.VisitVarStmt(Stmt.Var stmt)
        {
            if (stmt.initializer == null)
            {
                return ParenthesizeParts("var", stmt.name);
            }

            return ParenthesizeParts("var", stmt.name, "=", stmt.initializer);
        }

        string Stmt.IVisitor<string>.VisitWhileStmt(Stmt.While stmt)
        {
            return ParenthesizeParts("while", stmt.condition, stmt.body);
        }

        private string ParenthesizeParts(string name, params object[] parts)
        {
            StringBuilder builder = new();

            builder.Append("(").Append(name);
            Transform(builder, parts);
            builder.Append(")");

            return builder.ToString();
        }

        private void Transform(StringBuilder builder, params object[] parts)
        {
            foreach (object part in parts)
            {
                builder.Append(" ");
                switch (part)
                {
                    case Expr:
                        builder.Append(((Expr)part).Accept(this));
                        //> Statements and State omit
                        break;
                    case Stmt:
                        builder.Append(((Stmt)part).Accept(this));
                        //< Statements and State omit
                        break;
                    case Token:
                        builder.Append(((Token)part).lexeme);
                        break;
                    //else if (part is List)
                    //{
                    //    transform(builder, ((List)part).toArray());
                    //}
                    default:
                        builder.Append(part);
                        break;
                }
            }
        }

        string Expr.IVisitor<string>.VisitCallExpr(Expr.Call expr)
        {
            throw new NotImplementedException();
        }

        string Stmt.IVisitor<string>.VisitFunctionStmt(Stmt.Function stmt)
        {
            throw new NotImplementedException();
        }

        public string VisitReturnStmt(Stmt.Return stmt)
        {
            throw new NotImplementedException();
        }

        public string VisitClassStmt(Stmt.Class stmt)
        {
            throw new NotImplementedException();
        }

        public string VisitGetExpr(Expr.Get expr)
        {
            throw new NotImplementedException();
        }

        public string VisitSetExpr(Expr.Set expr)
        {
            throw new NotImplementedException();
        }

        public string VisitThisExpr(Expr.This expr)
        {
            throw new NotImplementedException();
        }
    }
}
