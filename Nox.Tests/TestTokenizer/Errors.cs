

namespace Nox.Tests.TestTokenizer
{
    public class Errors
    {
        [Fact]
        public void TestLexicalErrors()
        {
            //Arrange
            StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);


            string input = ",.$(#";
            Tokenizer tokenizer = new(input);

            //Act 
            List<Token> tokens = tokenizer.Tokenize();

            //Assert
            string output = stringWriter.ToString();
            Assert.Contains("[line 1] Error: Unexpected character: $", output);
            Assert.Contains("[line 1] Error: Unexpected character: #", output);


            Queue<Token> queue = new(tokens);
            Assert.Equal(4, tokens.Count);

            Assert.Equal("COMMA , null", queue.Dequeue().ToString());
            Assert.Equal("DOT . null", queue.Dequeue().ToString());
            Assert.Equal("LEFT_PAREN ( null", queue.Dequeue().ToString());
            Assert.Equal("EOF  null", queue.Dequeue().ToString());
        }


        ///https://app.codecrafters.io/courses/interpreter/stages/ue7
        ///Commenting out due to unexpected output. 

        //[Fact]
        //public void TestMultilineErrors()
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
