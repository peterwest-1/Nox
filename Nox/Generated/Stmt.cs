using Nox;

public abstract class Stmt
{
    public interface IVisitor<T>
    {
        T VisitBlockStmt(Block stmt);
        T VisitExpressionStmt(Expression stmt);
        T VisitPrintStmt(Print stmt);
        T VisitVarStmt(Var stmt);
    }
    public class Block : Stmt
    {
        public Block(List<Stmt> statements)
        {
            this.statements = statements;
        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitBlockStmt(this);
        }

        public List<Stmt> statements;
    }
    public class Expression : Stmt
    {
        public Expression(Expr expression)
        {
            this.expression = expression;
        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitExpressionStmt(this);
        }

        public Expr expression;
    }
    public class Print : Stmt
    {
        public Print(Expr expression)
        {
            this.expression = expression;
        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitPrintStmt(this);
        }

        public Expr expression;
    }
    public class Var : Stmt
    {
        public Var(Token name, Expr initializer)
        {
            this.name = name;
            this.initializer = initializer;
        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitVarStmt(this);
        }

        public Token name;
        public Expr initializer;
    }

    public abstract T Accept<T>(IVisitor<T> visitor);
}
