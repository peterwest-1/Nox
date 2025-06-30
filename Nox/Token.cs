
using System.Globalization;

namespace Nox
{
    public class Token
    {
        private readonly TokenType type;
        public readonly string lexeme;
        private readonly object? literal;
        private readonly int line;

        public Token(TokenType type, string lexeme, object? literal, int line)
        {
            this.type = type;
            this.lexeme = lexeme;
            this.literal = literal;
            this.line = line;
        }

        public override string ToString()
        {
            string? lit = literal switch
            {
                null => "null",
                double d when type == TokenType.NUMBER => d.ToString("0.0###", CultureInfo.InvariantCulture),
                _ => literal.ToString()
            };

            return $"{type} {lexeme} {lit}";
        }
    }
}
