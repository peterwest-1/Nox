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
                Environment.Exit(1);
            }

            string secondArgument = args[1];
            switch (command)
            {
                case "tokenize":
                    HandleTokenize(filename: secondArgument);
                    break;
                case "parse":
                    break;
                case "evaluate":
                    break;
                case "run":
                    break;
                default:
                    break;
            }
        }

        private static void HandleTokenize(string filename)
        {
            Nox.RunFile(filename);
        }
    }
}
