

using Nox.AST;

namespace Nox.Tests.TestParser
{
    public class Booleans
    {
        [Fact]
        public void TestParsePrintBooleansEvaluted()
        {
            //Arrange
            string input = "2 + 3";
            Tokenizer tokenizer = new(input);
            List<Token> tokens = tokenizer.Tokenize();

            //Act 
            Parser parser = new(tokens);
            Expr expression = parser.Parse();
            string result = new Printer().Print(expression);

            //Assert
            Assert.Equal("(+ 2.0 3.0)", result);

        }

        [Fact]
        public void TestParsePrintBooleans()
        {
            //Arrange
            string input = "true";
            Tokenizer tokenizer = new(input);
            List<Token> tokens = tokenizer.Tokenize();

            //Act 
            Parser parser = new(tokens);
            Expr expression = parser.Parse();
            string result = new Printer().Print(expression);

            //Assert
            Assert.Equal("true", result);

        }



    }
}
