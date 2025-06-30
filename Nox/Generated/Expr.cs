using Nox;

public abstract class Expr
{
    class Binary : Expr
    {
        public Binary(Expr left, Token op, Expr right)
        {
            this.left = left;
            this.op = op;
            this.right = right;
        }

        public Expr left;
        public Token op;
        public Expr right;
    }
    class Grouping : Expr
    {
        public Grouping(Expr expression)
        {
            this.expression = expression;
        }

        public Expr expression;
    }
    class Literal : Expr
    {
        public Literal(Object value)
        {
            this.value = value;
        }

        public Object value;
    }
    class Unary : Expr
    {
        public Unary(Token op, Expr right)
        {
            this.op = op;
            this.right = right;
        }

        public Token op;
        public Expr right;
    }
}
