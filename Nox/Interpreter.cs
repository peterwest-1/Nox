

using Nox.Callables;
using System;

namespace Nox
{
    internal class Interpreter : Expr.IVisitor<object>, Stmt.IVisitor<object>
    {
        public readonly Environment globals = new();
        private Environment environment;

        private Dictionary<Expr, int> locals = [];

        public Interpreter()
        {
            environment = globals;
            globals.Define(ClockCallable.name, new ClockCallable());
        }

        public void Interpret(List<Stmt> statements)
        {
            try
            {
                foreach (Stmt statement in statements)
                {
                    Execute(statement);
                }
            }
            catch (NoxRuntimeException ex)
            {
                Nox.RuntimeException(ex);
            }
        }

        private void Execute(Stmt stmt)
        {
            stmt.Accept(this);
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

            return ob.ToString() ?? "STRINGIFY PROBLEM IN INTERPRETER";
        }

        public object VisitExpressionStmt(Stmt.Expression stmt)
        {
            Evaluate(stmt.expression);
            return null;
        }

        public object VisitPrintStmt(Stmt.Print stmt)
        {
            object value = Evaluate(stmt.expression);
            Console.WriteLine(Stringify(value));
            return null;
        }

        object Expr.IVisitor<object>.VisitVariableExpr(Expr.Variable expr)
        {
            return LookUpVariable(expr.name, expr);
        }

        object Stmt.IVisitor<object>.VisitVarStmt(Stmt.Var stmt)
        {
            object value = null;
            if (stmt.initializer != null)
            {
                value = Evaluate(stmt.initializer);
            }

            environment.Define(stmt.name.lexeme, value);
            return null;
        }

        object Expr.IVisitor<object>.VisitAssignExpr(Expr.Assign expr)
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

        object Stmt.IVisitor<object>.VisitBlockStmt(Stmt.Block stmt)
        {
            ExecuteBlock(stmt.statements, new Environment(environment));
            return null;
        }

        public void ExecuteBlock(List<Stmt> statements, Environment environment)
        {
            Environment previous = this.environment;
            try
            {
                this.environment = environment;

                foreach (Stmt statement in statements)
                {
                    Execute(statement);
                }
            }
            finally
            {
                this.environment = previous;
            }
        }

        object Stmt.IVisitor<object>.VisitIfStmt(Stmt.If stmt)
        {
            if (IsTruthy(Evaluate(stmt.condition)))
            {
                Execute(stmt.thenBranch);
            }
            else if (stmt.elseBranch != null)
            {
                Execute(stmt.elseBranch);
            }
            return null;
        }

        object Expr.IVisitor<object>.VisitLogicalExpr(Expr.Logical expr)
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

        public object VisitWhileStmt(Stmt.While stmt)
        {
            while (IsTruthy(Evaluate(stmt.condition)))
            {
                Execute(stmt.body);
            }
            return null;
        }

        object Expr.IVisitor<object>.VisitCallExpr(Expr.Call expr)
        {
            object callee = Evaluate(expr.callee);

            List<object> arguments = [];
            foreach (Expr argument in expr.arguments)
            {
                arguments.Add(Evaluate(argument));
            }

            if (callee is not INoxCallable)
            {
                throw new NoxRuntimeException(expr.paren,
                    "Can only call functions and classes.");
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

        object Stmt.IVisitor<object>.VisitFunctionStmt(Stmt.Function stmt)
        {
            NoxFunction function = new(stmt, environment, false);
            environment.Define(stmt.name.lexeme, function);
            return null;
        }

        public object VisitReturnStmt(Stmt.Return stmt)
        {
            object value = null;
            if (stmt.value != null) value = Evaluate(stmt.value);

            throw new NoxReturnException(value);
        }

        public void Resolve(Expr expr, int depth)
        {
            locals.Add(expr, depth);
        }

        private object LookUpVariable(Token name, Expr expr)
        {
            if (locals.TryGetValue(expr, out int distance))
            {
                return environment.GetAt(distance, name.lexeme);
            }
            else
            {
                return globals.Get(name);
            }
        }

        public object VisitClassStmt(Stmt.Class stmt)
        {
            object superclass = null;
            if (stmt.superclass != null)
            {
                superclass = Evaluate(stmt.superclass);
                if ((superclass is not NoxClass))
                {
                    throw new NoxRuntimeException(stmt.superclass.name,
                        "Superclass must be a class.");
                }
            }

            environment.Define(stmt.name.lexeme, null);

            if (stmt.superclass != null)
            {
                environment = new Environment(environment);
                environment.Define(Constants.SUPER, superclass);
            }



            Dictionary<string, NoxFunction> methods = [];
            foreach (Stmt.Function method in stmt.methods)
            {
                NoxFunction function = new(method, environment, method.name.lexeme.Equals(Constants.INIT));
                methods[method.name.lexeme] = function;
            }

            NoxClass klass = new(stmt.name.lexeme, (NoxClass)superclass, methods);

            if (superclass != null)
            {
                environment = environment.enclosing;
            }

            environment.Assign(stmt.name, klass);
            return null;
        }

        public object VisitGetExpr(Expr.Get expr)
        {
            object obj = Evaluate(expr.obj);
            if (obj is NoxInstance ob)
            {
                return ob.Get(expr.name);
            }

            throw new NoxRuntimeException(expr.name,
                "Only instances have properties.");
        }

        public object VisitSetExpr(Expr.Set expr)
        {
            object obj = Evaluate(expr.obj);

            if (obj is not NoxInstance)
            {
                throw new NoxRuntimeException(expr.name,
                                       "Only instances have fields.");
            }

            object value = Evaluate(expr.value);
            ((NoxInstance)obj).Set(expr.name, value);
            return value;
        }

        public object VisitThisExpr(Expr.This expr)
        {
            return LookUpVariable(expr.keyword, expr);
        }

        object Expr.IVisitor<object>.VisitSuperExpr(Expr.Super expr)
        {
            int distance = locals[expr];
            NoxClass superclass = (NoxClass)environment.GetAt(
                distance, "super");

            NoxInstance obj = (NoxInstance)environment.GetAt(
      distance - 1, "this");

            NoxFunction method = superclass.FindMethod(expr.method.lexeme);

            if (method == null)
            {
                throw new NoxRuntimeException(expr.method,
                    "Undefined property '" + expr.method.lexeme + "'.");
            }

            return method.Bind(obj);
        }
    }
}

