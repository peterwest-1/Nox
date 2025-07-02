
namespace Nox.InterpreterImpl
{
    internal partial class Interpreter : Expr.IVisitor<object>
    {
        public object VisitAssignExpr(Expr.Assign expr)
        {
            object value = Evaluate(expr.value);
            int distance = locals.GetValueOrDefault(expr);
            if (distance != null)
            {
                environment.AssignAt(distance, expr.name, value);
            }
            else
            {
                globals.Assign(expr.name, value);
            }

            return value;
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
        public object VisitCallExpr(Expr.Call expr)
        {
            object callee = Evaluate(expr.callee);

            List<object> arguments = [];
            foreach (Expr argument in expr.arguments)
            {
                arguments.Add(Evaluate(argument));
            }

            if (callee is not INoxCallable)
            {
                throw new NoxRuntimeException(expr.paren, "Can only call functions and classes.");
            }

            INoxCallable function = (INoxCallable)callee;

            if (arguments.Count != function.Arity())
            {
                throw new NoxRuntimeException(expr.paren, "Expected " +
                    function.Arity() + " arguments but got " +
                    arguments.Count + ".");
            }

            return function.Call(this, arguments);
        }

        public object VisitGetExpr(Expr.Get expr)
        {
            object obj = Evaluate(expr.obj);
            if (obj is NoxInstance ob)
            {
                return ob.Get(expr.name);
            }

            throw new NoxRuntimeException(expr.name, "Only instances have properties.");
        }

        public object VisitGroupingExpr(Expr.Grouping expr)
        {
            return Evaluate(expr.expression);
        }

        public object VisitLiteralExpr(Expr.Literal expr)
        {
            return expr.value;
        }

        public object VisitLogicalExpr(Expr.Logical expr)
        {
            object left = Evaluate(expr.left);

            if (expr.op.type == TokenType.OR)
            {
                if (IsTruthy(left)) return left;
            }
            else
            {
                if (!IsTruthy(left)) return left;
            }

            return Evaluate(expr.right);
        }

        public object VisitSetExpr(Expr.Set expr)
        {
            object obj = Evaluate(expr.obj);

            if (obj is not NoxInstance)
            {
                throw new NoxRuntimeException(expr.name, "Only instances have fields.");
            }

            object value = Evaluate(expr.value);
            ((NoxInstance)obj).Set(expr.name, value);
            return value;
        }

        public object VisitSuperExpr(Expr.Super expr)
        {
            int distance = locals[expr];

            NoxClass superclass = (NoxClass)environment.GetAt(distance, Keywords.SUPER);
            NoxInstance obj = (NoxInstance)environment.GetAt(distance - 1, Keywords.THIS);
            NoxFunction method = superclass.FindMethod(expr.method.lexeme);

            if (method == null)
            {
                throw new NoxRuntimeException(expr.method, "Undefined property '" + expr.method.lexeme + "'.");
            }

            return method.Bind(obj);
        }

        public object VisitThisExpr(Expr.This expr)
        {
            return LookUpVariable(expr.keyword, expr);
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

        public object VisitVariableExpr(Expr.Variable expr)
        {
            return LookUpVariable(expr.name, expr);
        }
    }
}
