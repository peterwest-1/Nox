
using System.Text;

namespace Nox.AST
{
    internal partial class Printer
    {
        public string Print(Expr expr)
        {
            return expr.Accept(this);
        }

        public string Print(Stmt stmt)
        {
            return stmt.Accept(this);
        }
        public string Print(List<Stmt> statements)
        {
            var builder = new StringBuilder();

            foreach (var stmt in statements)
            {
                builder.AppendLine(Print(stmt));
            }

            return builder.ToString();
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

        private string ParenthesizeParts(string name, params object[] parts)
        {
            StringBuilder builder = new();

            builder.Append('(').Append(name);
            Transform(builder, parts);
            builder.Append(')');

            return builder.ToString();
        }

        private void Transform(StringBuilder builder, params object[] parts)
        {
            foreach (object part in parts)
            {
                builder.Append(' ');
                switch (part)
                {
                    case Expr:
                        builder.Append(((Expr)part).Accept(this));
                        break;
                    case Stmt:
                        builder.Append(((Stmt)part).Accept(this));
                        break;
                    case Token:
                        builder.Append(((Token)part).lexeme);
                        break;
                    case List<Stmt> v1:
                        Transform(builder, (v1).ToList());
                        break;
                    default:
                        builder.Append(part);
                        break;
                }
            }
        }

    }
}
