using Nox;

public abstract class Expr
{
    public interface IVisitor<T>
    {
        T VisitAssignExpr(Assign expr);
        T VisitBinaryExpr(Binary expr);
        T VisitCallExpr(Call expr);
        T VisitGetExpr(Get expr);
        T VisitGroupingExpr(Grouping expr);
        T VisitLiteralExpr(Literal expr);
        T VisitLogicalExpr(Logical expr);
        T VisitSetExpr(Set expr);
        T VisitThisExpr(This expr);
        T VisitUnaryExpr(Unary expr);
        T VisitVariableExpr(Variable expr);
    }
    public class Assign : Expr
    {
        public Assign(Token name, Expr value)
        {
            this.name = name;
            this.value = value;
        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitAssignExpr(this);
        }

        public Token name;
        public Expr value;
    }
    public class Binary : Expr
    {
        public Binary(Expr left, Token op, Expr right)
        {
            this.left = left;
            this.op = op;
            this.right = right;
        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitBinaryExpr(this);
        }

        public Expr left;
        public Token op;
        public Expr right;
    }
    public class Call : Expr
    {
        public Call(Expr callee, Token paren, List<Expr> arguments)
        {
            this.callee = callee;
            this.paren = paren;
            this.arguments = arguments;
        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitCallExpr(this);
        }

        public Expr callee;
        public Token paren;
        public List<Expr> arguments;
    }
    public class Get : Expr
    {
        public Get(Expr obj, Token name)
        {
            this.obj = obj;
            this.name = name;
        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitGetExpr(this);
        }

        public Expr obj;
        public Token name;
    }
    public class Grouping : Expr
    {
        public Grouping(Expr expression)
        {
            this.expression = expression;
        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitGroupingExpr(this);
        }

        public Expr expression;
    }
    public class Literal : Expr
    {
        public Literal(object value)
        {
            this.value = value;
        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitLiteralExpr(this);
        }

        public object value;
    }
    public class Logical : Expr
    {
        public Logical(Expr left, Token op, Expr right)
        {
            this.left = left;
            this.op = op;
            this.right = right;
        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitLogicalExpr(this);
        }

        public Expr left;
        public Token op;
        public Expr right;
    }
    public class Set : Expr
    {
        public Set(Expr obj, Token name, Expr value)
        {
            this.obj = obj;
            this.name = name;
            this.value = value;
        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitSetExpr(this);
        }

        public Expr obj;
        public Token name;
        public Expr value;
    }
    public class This : Expr
    {
        public This(Token keyword)
        {
            this.keyword = keyword;
        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitThisExpr(this);
        }

        public Token keyword;
    }
    public class Unary : Expr
    {
        public Unary(Token op, Expr right)
        {
            this.op = op;
            this.right = right;
        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitUnaryExpr(this);
        }

        public Token op;
        public Expr right;
    }
    public class Variable : Expr
    {
        public Variable(Token name)
        {
            this.name = name;
        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitVariableExpr(this);
        }

        public Token name;
    }

    public abstract T Accept<T>(IVisitor<T> visitor);
}
