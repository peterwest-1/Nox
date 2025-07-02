

using System;

namespace Nox
{
    //Environment is already in .NET, changing to "Habitat"
    internal class Environment
    {
        public Environment? enclosing;
        private readonly Dictionary<string, object> values = [];

        public Environment()
        {
            enclosing = null;
        }

        public Environment(Environment enclosing)
        {
            this.enclosing = enclosing;
        }

        public void Define(string name, object value)
        {
            values[name] = value;
        }

        public object Get(Token name)
        {
            if (values.TryGetValue(name.lexeme, out object? value))
            {
                return value;
            }

            if (enclosing != null) return enclosing.Get(name);

            throw new NoxRuntimeException(name,
                "Undefined variable '" + name.lexeme + "'.");
        }

        public void Assign(Token name, object value)
        {
            if (values.ContainsKey(name.lexeme))
            {
                values[name.lexeme] = value;
                return;
            }

            if (enclosing != null)
            {
                enclosing.Assign(name, value);
                return;
            }

            throw new NoxRuntimeException(name,
                "Undefined variable '" + name.lexeme + "'.");
        }

        public object GetAt(int distance, string name)
        {
            return Ancestor(distance).values.GetValueOrDefault(name);
        }

        private Environment Ancestor(int distance)
        {
            Environment environment = this;
            for (int i = 0; i < distance; i++)
            {
                environment = environment.enclosing;
            }

            return environment;
        }

        public void AssignAt(int distance, Token name, Object value)
        {
            Ancestor(distance).values[name.lexeme] = value;
        }
    }
}
