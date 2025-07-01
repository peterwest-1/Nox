
using System.Globalization;

namespace Nox
{
    internal class Tokenizer
    {
        private string source;
        private List<Token> tokens = new();

        private static readonly Dictionary<string, TokenType> keywords = new()
        {
            ["and"] = TokenType.AND,
            ["class"] = TokenType.CLASS,
            ["else"] = TokenType.ELSE,
            ["false"] = TokenType.FALSE,
            ["for"] = TokenType.FOR,
            ["fun"] = TokenType.FUN,
            ["if"] = TokenType.IF,
            ["nil"] = TokenType.NIL,
            ["or"] = TokenType.OR,
            ["print"] = TokenType.PRINT,
            ["return"] = TokenType.RETURN,
            ["super"] = TokenType.SUPER,
            ["this"] = TokenType.THIS,
            ["true"] = TokenType.TRUE,
            ["var"] = TokenType.VAR,
            ["while"] = TokenType.WHILE
        };

        private int start = 0;
        private int current = 0;
        private int line = 1;

        public Tokenizer(string source)
        {
            this.source = source;
        }

        private bool IsAtEnd => current >= source.Length;

        public List<Token> Tokenize()
        {
            while (!IsAtEnd)
            {
                // We are at the beginning of the next lexeme.
                start = current;
                ScanToken();
            }

            tokens.Add(new Token(TokenType.EOF, "", null, line));
            return tokens;
        }

        private void ScanToken()
        {
            char c = Advance();
            switch (c)
            {
                case '(': AddToken(TokenType.LEFT_PAREN); break;
                case ')': AddToken(TokenType.RIGHT_PAREN); break;
                case '{': AddToken(TokenType.LEFT_BRACE); break;
                case '}': AddToken(TokenType.RIGHT_BRACE); break;
                case ',': AddToken(TokenType.COMMA); break;
                case '.': AddToken(TokenType.DOT); break;
                case '-': AddToken(TokenType.MINUS); break;
                case '+': AddToken(TokenType.PLUS); break;
                case ';': AddToken(TokenType.SEMICOLON); break;
                case '*': AddToken(TokenType.STAR); break;
                case '!':
                    AddToken(Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG);
                    break;
                case '=':
                    AddToken(Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
                    break;
                case '<':
                    AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS);
                    break;
                case '>':
                    AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
                    break;
                case '/':
                    if (Match('/'))
                    {
                        // A comment goes until the end of the line.
                        while (Peek() != '\n' && !IsAtEnd) { Advance(); }
                    }
                    else
                    {
                        AddToken(TokenType.SLASH);
                    }
                    break;
                case ' ':
                case '\r':
                case '\t':
                    // Ignore whitespace.
                    break;
                case '\n':
                    line++;
                    break;
                case '"': ScanString(); break;
                default:
                    if (IsDigit(c))
                    {
                        ScanNumber();
                    }
                    else if (IsAlpha(c))
                    {
                        ScanIdentifier();
                    }
                    else
                    {
                        Nox.Error(line, $"Unexpected character: {c}");
                    }
                    break;
            }
        }

        private void ScanIdentifier()
        {
            while (IsAlphaNumeric(Peek())) { Advance(); }
            string text = source[start..current];
            AddToken(keywords.GetValueOrDefault(text, TokenType.IDENTIFIER));
        }


        private void ScanString()
        {
            while (Peek() != '"' && !IsAtEnd)
            {
                if (Peek() == '\n') line++;
                Advance();
            }

            if (IsAtEnd)
            {
                Nox.Error(line, "Unterminated string.");
                return;
            }

            // The closing ".
            Advance();

            // Trim the surrounding quotes.
            string value = source[(start + 1)..(current - 1)];
            AddToken(TokenType.STRING, value);
        }

        private void ScanNumber()
        {
            while (IsDigit(Peek())) { Advance(); }

            // Look for a fractional part.
            if (Peek() == '.' && IsDigit(PeekNext()))
            {
                // Consume the "."
                Advance();

                while (IsDigit(Peek())) { Advance(); }
            }

            string number = source[start..current];
            if (double.TryParse(number, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
            {
                AddToken(TokenType.NUMBER, value);
            }
            else
            {
                throw new Exception($"Invalid number format: '{number}'");
            }
        }


        private bool Match(char expected)
        {
            if (IsAtEnd) return false;
            if (source.ElementAt(current) != expected) return false;

            current++;
            return true;
        }

        private char Peek()
        {
            if (IsAtEnd) return '\0';
            return source.ElementAt(current);
        }

        private char PeekNext()
        {
            if (current + 1 >= source.Length) return '\0';
            return source.ElementAt(current + 1);
        }

        private char Advance()
        {
            return source.ElementAt(current++);
        }

        private void AddToken(TokenType type)
        {
            AddToken(type, null);
        }

        private void AddToken(TokenType type, object? literal)
        {
            string text = source[start..current];
            tokens.Add(new Token(type, text, literal, line));
        }

        private static bool IsDigit(char c) => c >= '0' && c <= '9';

        private static bool IsAlpha(char c) => (c >= 'a' && c <= 'z') ||
                   (c >= 'A' && c <= 'Z') ||
                    c == '_';

        private static bool IsAlphaNumeric(char c) => IsAlpha(c) || IsDigit(c);

    }
}
