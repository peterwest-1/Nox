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
                System.Environment.Exit(1);
            }

            string secondArgument = args[1];
            switch (command)
            {
                case "tokenize":
                    Nox.TokenizeFile(filename: secondArgument);
                    break;
                case "parse":
                    Nox.ParseFile(filename: secondArgument);
                    break;
                case "evaluate":
                    Nox.EvaluateFile(filename: secondArgument);
                    break;
                case "run":
                    throw new NotImplementedException();
                case "genast":
                    Generator.Generate(outputDirectory: secondArgument);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

    }
}
