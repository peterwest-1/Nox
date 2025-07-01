
using Nox.AST;

namespace Nox
{
    internal class Nox
    {
        private static bool hadError = false;

        public static bool HadError { get => hadError; set => hadError = value; }

        public static void RunFile(string path)
        {
            string contents = File.ReadAllText(Path.GetFullPath(path));
            Run(contents);
            if (hadError) { Environment.Exit(65); }

        }

        public static void TokenizeFile(string filename)
        {
            string contents = File.ReadAllText(Path.GetFullPath(filename));
            Tokenizer tokenizer = new(contents);
            List<Token> tokens = tokenizer.Tokenize();

            // For now, just print the tokens.
            foreach (Token token in tokens)
            {
                Console.WriteLine(token);
            }

        }

        public static void ParseFile(string filename)
        {
            string contents = File.ReadAllText(Path.GetFullPath(filename));
            Tokenizer tokenizer = new(contents);
            List<Token> tokens = tokenizer.Tokenize();

            Parser parser = new(tokens);
            Expr expression = parser.Parse();

            if (hadError) return;

            Console.WriteLine(new Printer().Print(expression));
        }

        private static void RunPrompt()
        {
            for (; ; )
            {
                Console.Write("> ");
                string? line = Console.ReadLine();
                if (line == null) break;
                Run(line);
            }
        }

        private static void Run(string source)
        {
            Tokenizer tokenizer = new(source);
            List<Token> tokens = tokenizer.Tokenize();

            // For now, just print the tokens.
            foreach (Token token in tokens)
            {
                Console.WriteLine(token);
            }
        }
        public static void Error(int line, string message)
        {
            Report(line, "", message);
        }

        public static void Error(Token token, String message)
        {
            if (token.type == TokenType.EOF)
            {
                Report(token.line, " at end", message);
            }
            else
            {
                Report(token.line, " at '" + token.lexeme + "'", message);
            }
        }

        private static void Report(int line, string where, string message)
        {
            Console.WriteLine("[line " + line + "] Error" + where + ": " + message);
            hadError = true;
        }

    }
}
