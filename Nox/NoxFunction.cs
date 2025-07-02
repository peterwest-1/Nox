
using static Stmt;

namespace Nox
{
    internal class NoxFunction : INoxCallable
    {
        private Function declaration;
        private Environment closure;
        private readonly bool IsInitializer;

        public NoxFunction(Function declaration, Environment closure, bool IsInitializer)
        {
            this.declaration = declaration;
            this.closure = closure;
            this.IsInitializer = IsInitializer;
        }

        public int Arity() => declaration.paras.Count;

        public object Call(Interpreter interpreter, List<object> arguments)
        {
            Environment environment = new(closure);
            for (int i = 0; i < declaration.paras.Count; i++)
            {
                environment.Define(declaration.paras.ElementAt(i).lexeme,
                    arguments.ElementAt(i));
            }

            try
            {
                interpreter.ExecuteBlock(declaration.body, environment);
            }
            catch (NoxReturnException returnValue)
            {
                if (IsInitializer) return closure.GetAt(0, Keywords.THIS);
                return returnValue.value;
            }

            if (IsInitializer) return closure.GetAt(0, Keywords.THIS);
            return null;
        }

        override public string ToString()
        {
            return "<fn " + declaration.name.lexeme + ">";
        }

        public NoxFunction Bind(NoxInstance instance)
        {
            Environment environment = new(closure);
            environment.Define(Keywords.THIS, instance);
            return new NoxFunction(declaration, environment, IsInitializer);
        }
    }
}
