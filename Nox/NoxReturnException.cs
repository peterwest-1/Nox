
namespace Nox
{
    [Serializable]
    internal class NoxReturnException : Exception
    {
        public object? value;

        public NoxReturnException()
        {
        }

        public NoxReturnException(object? value)
        {
            this.value = value;
        }

        public NoxReturnException(string? message) : base(message)
        {
        }

        public NoxReturnException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}