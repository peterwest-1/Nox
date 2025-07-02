
namespace Nox
{
    internal interface INoxCallable
    {
        int Arity();
        object Call(Interpreter interpreter, List<object> arguments);
    }
}
