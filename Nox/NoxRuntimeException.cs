
namespace Nox
{
    [Serializable]
    internal class NoxRuntimeException : Exception
    {
        private Token token;
        private string reason;

        public NoxRuntimeException()
        {
        }

        public NoxRuntimeException(string? message) : base(message)
        {
        }

        public NoxRuntimeException(Token op, string reason) : base(reason)
        {
            token = op;
        }

        public NoxRuntimeException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        public Token Token { get => token; set => token = value; }
    }
}