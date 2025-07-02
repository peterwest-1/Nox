
namespace Nox
{
    internal class NoxClass : INoxCallable
    {
        public string name;

        private Dictionary<string, NoxFunction> methods;

        public NoxClass(string name, Dictionary<string, NoxFunction> methods)
        {
            this.name = name;
            this.methods = methods;
        }

        public int Arity()
        {
            NoxFunction initializer = FindMethod(Constants.INIT);
            if (initializer == null) return 0;
            return initializer.Arity();
        }

        public object Call(Interpreter interpreter, List<object> arguments)
        {
            NoxInstance instance = new(this);
            NoxFunction initializer = FindMethod(Constants.INIT);
            if (initializer != null)
            {
                initializer.Bind(instance).Call(interpreter, arguments);
            }
            return instance;
        }

        public NoxFunction FindMethod(string name)
        {
            if (methods.ContainsKey(name))
            {
                return methods.GetValueOrDefault(name);
            }

            return null;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
