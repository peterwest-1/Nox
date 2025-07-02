
namespace Nox
{
    internal class NoxClass : INoxCallable
    {
        public string name;
        public NoxClass superclass;
        private Dictionary<string, NoxFunction> methods;

        public NoxClass(string name, NoxClass superclass, Dictionary<string, NoxFunction> methods)
        {
            this.name = name;
            this.superclass = superclass;
            this.methods = methods;
        }

        public int Arity()
        {
            NoxFunction initializer = FindMethod(Keywords.INIT);
            if (initializer == null) return 0;
            return initializer.Arity();
        }

        public object Call(Interpreter interpreter, List<object> arguments)
        {
            NoxInstance instance = new(this);
            NoxFunction initializer = FindMethod(Keywords.INIT);
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

            if (superclass != null)
            {
                return superclass.FindMethod(name);
            }

            return null;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
