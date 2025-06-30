

namespace Nox.Tests.TestTokenizer
{
    public class Whitespace
    {
        [Fact]
        public void TestWhitespaceAll()
        {
            //Arrange
            string input = "space    tabs\t\t\t\tnewlines\r\n\r\n\r\n\r\n\r\nend";
            Tokenizer tokenizer = new(input);

            //Act 
            List<Token> tokens = tokenizer.Tokenize();

            //Assert
            Queue<Token> queue = new(tokens);
            Assert.Equal(5, tokens.Count);

            Assert.Equal("IDENTIFIER space null", queue.Dequeue().ToString());
            Assert.Equal("IDENTIFIER tabs null", queue.Dequeue().ToString());
            Assert.Equal("IDENTIFIER newlines null", queue.Dequeue().ToString());
            Assert.Equal("IDENTIFIER end null", queue.Dequeue().ToString());
            Assert.Equal("EOF  null", queue.Dequeue().ToString());
        }
    }
}
