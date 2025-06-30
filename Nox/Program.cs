using Nox.AST;

namespace Nox
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //tokenize
            //parse
            //evaluate
            //run
            string command = args[0];
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: tokenize <filename.nox>");
                Console.WriteLine("Usage: parse <filename.nox>");
                Environment.Exit(1);
            }

            string secondArgument = args[1];
            switch (command)
            {
                case "tokenize":
                    HandleTokenize(filename: secondArgument);
                    break;
                case "parse":
                    Expr expression = new Expr.Binary(
                        new Expr.Unary(
                            new Token(TokenType.MINUS, "-", null, 1),
                            new Expr.Literal(123)),
                        new Token(TokenType.STAR, "*", null, 1),
                        new Expr.Grouping(
                            new Expr.Literal(45.67)));

                    Console.WriteLine(new Printer().Print(expression));
                    break;
                case "evaluate":
                    break;
                case "run":
                    break;
                case "genast":
                    HandleGenerateAST(outputDirectory: secondArgument);
                    break;
                default:
                    break;
            }
        }

        private static void HandleTokenize(string filename)
        {
            Nox.RunFile(filename);
        }

        private static void HandleGenerateAST(string outputDirectory)
        {
            Generator.Generate(outputDirectory);
        }
    }
}
