
using Nox.AST;
using System.Linq.Expressions;

namespace Nox
{
    internal class Nox
    {

        private static Interpreter interpreter = new();

        private static bool hadError = false;
        public static bool HadError { get => hadError; set => hadError = value; }

        private static bool hadRuntimeError = false;
        public static bool HadRuntimeError { get => hadRuntimeError; set => hadRuntimeError = value; }

        public static void RunFile(string path)
        {
            string contents = File.ReadAllText(Path.GetFullPath(path));
            Run(contents);
            if (hadError) { Environment.Exit(65); }
            if (HadRuntimeError) { Environment.Exit(70); }

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
            if (hadError) return;

            Parser parser = new(tokens);
            List<Stmt> statements = parser.Parse();
            if (hadError) return;

            //Console.WriteLine(new Printer().Print(statements));
        }

        public static void EvaluateFile(string filename)
        {
            string contents = File.ReadAllText(Path.GetFullPath(filename));
            Tokenizer tokenizer = new(contents);
            List<Token> tokens = tokenizer.Tokenize();
            if (hadError) return;

            Parser parser = new(tokens);
            List<Stmt> statements = parser.Parse();
            if (hadError) return;

            interpreter.Interpret(statements);
            if (hadError) return;
            if (hadRuntimeError) return; //?
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
            if (hadError) return;

            Parser parser = new(tokens);
            List<Stmt> statements = parser.Parse();
            if (hadError) return;

            interpreter.Interpret(statements);
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

        public static void RuntimeException(NoxRuntimeException exception)
        {
            Console.Error.WriteLine(exception.Message +
                "\n[line " + exception.Token.line + "]");
            hadRuntimeError = true;
        }

    }
}
