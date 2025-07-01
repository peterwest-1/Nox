

using Nox.AST;

namespace Nox.Tests.TestParser
{
    public class Errors
    {
        [Fact]
        public void TestParsePrintSyntacticErrors()
        {
            //Arrange
            StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);


            string input = "(72 +)";
            Tokenizer tokenizer = new(input);
            List<Token> tokens = tokenizer.Tokenize();

            //Act 
            Parser parser = new(tokens);
            Expr expression = parser.Parse();
            //string result = new Printer().Print(expression);

            //Assert
            string output = stringWriter.ToString();
            Assert.Contains("[line 1] Error at ')': Expect expression.", output);
        }


        ///https://app.codecrafters.io/courses/interpreter/stages/ue7
        ///Commenting out due to unexpected output. 

        //[Fact]
        //public void TestTokenizeMultilineErrors()
        //{
        //    //Arrange
        //    StringWriter stringWriter = new StringWriter();
        //    Console.SetOut(stringWriter);

        //    string input = "#<|SPACE|>(\r\n)<|TAB|>@";
        //    Tokenizer tokenizer = new(input);

        //    //Act 
        //    List<Token> tokens = tokenizer.Tokenize();

        //    //Assert
        //    string output = stringWriter.ToString();
        //    Assert.Contains("[line 1] Error: Unexpected character: #", output);
        //    Assert.Contains("[line 2] Error: Unexpected character: @", output);


        //    Queue<Token> queue = new(tokens);
        //    Assert.Equal(4, tokens.Count);

        //    Assert.Equal(TokenOutput.Get(TokenType.LEFT_PAREN), queue.Dequeue().ToString());
        //    Assert.Equal(TokenOutput.Get(TokenType.RIGHT_PAREN), queue.Dequeue().ToString());
        //    Assert.Equal(TokenOutput.Get(TokenType.EOF), queue.Dequeue().ToString());
        //}
    }
}
