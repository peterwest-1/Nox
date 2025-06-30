
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

        private static void Report(int line, string where, string message)
        {
            Console.WriteLine("[line " + line + "] Error" + where + ": " + message);
            hadError = true;
        }

    }
}
