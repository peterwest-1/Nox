using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Stmt;

namespace Nox
{
    internal class NoxInstance
    {
        private readonly NoxClass mClass;

        private readonly Dictionary<string, object> fields = [];

        public NoxInstance(NoxClass klass)
        {
            mClass = klass;
        }
        public object Get(Token name)
        {
            if (fields.TryGetValue(name.lexeme, out var value))
            {
                return value;
            }

            NoxFunction method = mClass.FindMethod(name.lexeme);
            if (method != null) return method.Bind(this);
            throw new NoxRuntimeException(name, $"Undefined property '{name.lexeme}'.");
        }

        public void Set(Token name, object value)
        {
            fields[name.lexeme] = value;
        }

        public override string ToString()
        {
            return $"{mClass.name} instance";
        }

    }
}
