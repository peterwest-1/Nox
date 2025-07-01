
namespace Nox
{
    internal class Interpreter : Expr.IVisitor<object>
    {

        public void Interpret(Expr expression)
        {
            try
            {
                object value = Evaluate(expression);
                Console.WriteLine(Stringify(value));
            }
            catch (NoxRuntimeException ex)
            {
                Nox.RuntimeError(ex);
            }
        }


        public object VisitBinaryExpr(Expr.Binary expr)
        {
            object left = Evaluate(expr.left);
            object right = Evaluate(expr.right);

            switch (expr.op.type)
            {
                case TokenType.GREATER:
                    CheckNumberOperands(expr.op, left, right);
                    return (double)left > (double)right;
                case TokenType.GREATER_EQUAL:
                    CheckNumberOperands(expr.op, left, right);
                    return (double)left >= (double)right;
                case TokenType.LESS:
                    CheckNumberOperands(expr.op, left, right);
                    return (double)left < (double)right;
                case TokenType.LESS_EQUAL:
                    CheckNumberOperands(expr.op, left, right);
                    return (double)left <= (double)right;
                case TokenType.MINUS:
                    CheckNumberOperands(expr.op, left, right);
                    return (double)left - (double)right;
                case TokenType.PLUS:
                    if (left is double lhs_d && right is double rhs_d) return lhs_d + rhs_d;
                    if (left is string lhs_s && right is string rhs_s) return lhs_s + rhs_s;
                    throw new NoxRuntimeException(expr.op, "Operands must be two numbers or two strings.");
                case TokenType.SLASH:
                    CheckNumberOperands(expr.op, left, right);
                    return (double)left / (double)right;
                case TokenType.STAR:
                    CheckNumberOperands(expr.op, left, right);
                    return (double)left * (double)right;
                case TokenType.BANG_EQUAL: return !IsEqual(left, right);
                case TokenType.EQUAL_EQUAL: return IsEqual(left, right);
                default:
                    throw new NoxRuntimeException(expr.op, "Operands must be two numbers or two strings.");
            }
        }

        public object VisitGroupingExpr(Expr.Grouping expr)
        {
            return Evaluate(expr.expression);
        }

        public object VisitLiteralExpr(Expr.Literal expr)
        {
            return expr.value;
        }

        public object VisitUnaryExpr(Expr.Unary expr)
        {
            object right = Evaluate(expr.right);

            switch (expr.op.type)
            {
                case TokenType.BANG:
                    return !IsTruthy(right);
                case TokenType.MINUS:
                    CheckNumberOperand(expr.op, right);
                    return -(double)right;
                default:
                    throw new NoxRuntimeException(expr.op, "Unknown unary operator.");
            }

        }
        private object Evaluate(Expr expr)
        {
            return expr.Accept(this);
        }

        private static bool IsTruthy(object ob)
        {
            if (ob == null) return false;
            return ob switch
            {
                bool => (bool)ob,
                _ => true
            };
        }

        private static bool IsEqual(object a, object b)
        {
            if (a == null && b == null) return true;
            if (a == null) return false;

            return a.Equals(b);
        }

        private static void CheckNumberOperand(Token op, object operand)
        {
            if (operand is double) return;

            throw new NoxRuntimeException(op, "Operand must be a number.");
        }

        private static void CheckNumberOperands(Token op, object left, object right)
        {
            if (left is double && right is double) return;
            throw new NoxRuntimeException(op, "Operands must be numbers.");
        }

        private static string Stringify(object ob)
        {

            if (ob == null) return "nil";
            if (ob is bool b) return b ? "true" : "false";
            if (ob is double)
            {
                string text = ob.ToString();
                if (text.EndsWith(".0"))
                {
                    text = text.Substring(0, text.Length - 2);
                }
                return text;
            }

            return ob.ToString();
        }
    }
}
