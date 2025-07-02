
using Nox.Callables;

namespace Nox.InterpreterImpl
{
    internal partial class Interpreter
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
    }
}
