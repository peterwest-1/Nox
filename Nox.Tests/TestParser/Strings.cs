

//using Nox.AST;

//namespace Nox.Tests.TestParser
//{
//    public class Strings
//    {
//        [Fact]
//        public void TestParsePrintStrings()
//        {
//            //Arrange
//            string input = "\"hello\"";
//            Tokenizer tokenizer = new(input);
//            List<Token> tokens = tokenizer.Tokenize();

//            //Act 
//            Parser parser = new(tokens);
//            Expr expression = parser.Parse();
//            string result = new Printer().Print(expression);

//            //Assert
//            Assert.Equal("hello", result);

//        }
//    }
//}
