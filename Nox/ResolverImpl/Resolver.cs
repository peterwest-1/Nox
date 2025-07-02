
using Nox.InterpreterImpl;

namespace Nox.ResolverImpl
{
    internal partial class Resolver(Interpreter interpreter)
    {
        enum ClassType
        {
            NONE,
            CLASS,
            SUBCLASS
        }

        private readonly Interpreter interpreter = interpreter;

        private readonly Stack<Dictionary<string, bool>> scopes = [];
        private FunctionType currentFunction = FunctionType.NONE;
        private ClassType currentClass = ClassType.NONE;

        private void Resolve(Expr expr) => expr.Accept(this);

        private void Resolve(Stmt stmt) => stmt.Accept(this);

        public void Resolve(List<Stmt> statements)
        {
            foreach (Stmt statement in statements)
            {
                Resolve(statement);
            }
        }

        private void BeginScope() => scopes.Push([]);

        private void EndScope() => scopes.Pop();

        private void Declare(Token name)
        {
            if (scopes.Count == 0) return;
            Dictionary<string, bool> scope = scopes.Peek();
            if (scope.ContainsKey(name.lexeme))
            {
                Nox.Error(name,
                    "Already a variable with this name in this scope.");
            }
            scope.Add(name.lexeme, false);
        }

        private void Define(Token name)
        {
            if (scopes.Count == 0) return;
            scopes.Peek()[name.lexeme] = true;
        }

        private void ResolveLocal(Expr expr, Token name)
        {
            for (int i = scopes.Count - 1; i >= 0; i--)
            {
                if (scopes.ElementAt(i).ContainsKey(name.lexeme))
                {
                    interpreter.Resolve(expr, scopes.Count - 1 - i);
                    return;
                }
            }
        }

        private void ResolveFunction(Stmt.Function function, FunctionType type)
        {
            FunctionType enclosingFunction = currentFunction;
            currentFunction = type;

            BeginScope();
            foreach (Token param in function.paras)
            {
                Declare(param);
                Define(param);
            }
            Resolve(function.body);
            EndScope();
            currentFunction = enclosingFunction;
        }
    }
}
