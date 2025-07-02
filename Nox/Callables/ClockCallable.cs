
using Nox.InterpreterImpl;

namespace Nox.Callables
{
    internal class ClockCallable : INoxCallable
    {

        public const string name = "clock";

        public int Arity() => 0;

        public object Call(Interpreter interpreter, List<object> arguments)
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() / 1000.0;
        }

        public override string ToString() => "<native fn>";
    }
}
