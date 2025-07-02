
using System.Globalization;

namespace Nox
{
    public class Token(TokenType type, string lexeme, object? literal, int line)
    {
        public readonly TokenType type = type;
        public readonly string lexeme = lexeme;
        public readonly object? literal = literal;
        public readonly int line = line;

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
