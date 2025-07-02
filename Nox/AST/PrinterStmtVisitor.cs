
using System.Text;

namespace Nox.AST
{
    internal partial class Printer : Stmt.IVisitor<string>
    {
        public string VisitBlockStmt(Stmt.Block stmt)
        {
            StringBuilder builder = new();
            builder.Append("(block ");

            foreach (Stmt statement in stmt.statements)
            {
                builder.Append(statement.Accept(this));
            }

            builder.Append(')');
            return builder.ToString();
        }

        public string VisitClassStmt(Stmt.Class stmt)
        {
            StringBuilder builder = new();
            builder.Append($"({Keywords.CLASS} " + stmt.name.lexeme);

            if (stmt.superclass != null)
            {
                builder.Append(" < " + Print(stmt.superclass));
            }

            foreach (Stmt.Function method in stmt.methods)
            {
                builder.Append(" " + Print(method));
            }

            builder.Append(')');
            return builder.ToString();
        }

        public string VisitExpressionStmt(Stmt.Expression stmt)
        {
            return Parenthesize(";", stmt.expression);
        }

        public string VisitFunctionStmt(Stmt.Function stmt)
        {
            StringBuilder builder = new();
            builder.Append($"({Keywords.FUNC} " + stmt.name.lexeme + "(");

            foreach (Token param in stmt.paras)
            {
                if (param != stmt.paras.ElementAt(0)) builder.Append(' ');
                builder.Append(param.lexeme);
            }

            builder.Append(") ");

            foreach (Stmt body in stmt.body)
            {
                builder.Append(body.Accept(this));
            }

            builder.Append(')');
            return builder.ToString();
        }

        public string VisitIfStmt(Stmt.If stmt)
        {
            if (stmt.elseBranch == null)
            {
                return ParenthesizeParts("if", stmt.condition, stmt.thenBranch);
            }

            return ParenthesizeParts("if-else", stmt.condition, stmt.thenBranch,
                stmt.elseBranch);
        }

        public string VisitPrintStmt(Stmt.Print stmt)
        {
            return Parenthesize("print", stmt.expression);
        }

        public string VisitReturnStmt(Stmt.Return stmt)
        {
            if (stmt.value == null) return "(return)";
            return Parenthesize("return", stmt.value);
        }

        public string VisitVarStmt(Stmt.Var stmt)
        {
            if (stmt.initializer == null)
            {
                return ParenthesizeParts("var", stmt.name);
            }

            return ParenthesizeParts("var", stmt.name, "=", stmt.initializer);
        }

        public string VisitWhileStmt(Stmt.While stmt)
        {
            return ParenthesizeParts("while", stmt.condition, stmt.body);
        }
    }
}
