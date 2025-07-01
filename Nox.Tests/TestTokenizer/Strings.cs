

namespace Nox.Tests.TestTokenizer
{
    public class Strings
    {
        [Fact]
        public void TestTokenizeStringsAll()
        {
            //Arrange
            string input = "\"\"\r\n\"string\"\r\n";
            Tokenizer tokenizer = new(input);

            //Act 
            List<Token> tokens = tokenizer.Tokenize();

            //Assert
            Queue<Token> queue = new(tokens);
            Assert.Equal(3, tokens.Count);

            Assert.Equal("STRING \"\" ", queue.Dequeue().ToString());
            Assert.Equal("STRING \"string\" string", queue.Dequeue().ToString());
            Assert.Equal("EOF  null", queue.Dequeue().ToString());

        }

        [Fact]
        public void TestTokenizeStringsEmpty()
        {
            //Arrange
            string input = "\"\"";
            Tokenizer tokenizer = new(input);

            //Act 
            List<Token> tokens = tokenizer.Tokenize();

            //Assert
            Queue<Token> queue = new(tokens);
            Assert.Equal(2, tokens.Count);

            Assert.Equal("STRING \"\" ", queue.Dequeue().ToString());
            Assert.Equal("EOF  null", queue.Dequeue().ToString());

        }

        [Fact]
        public void TestTokenizeStringsString()
        {
            //Arrange
            string input = "\"string\"";
            Tokenizer tokenizer = new(input);

            //Act 
            List<Token> tokens = tokenizer.Tokenize();

            //Assert
            Queue<Token> queue = new(tokens);
            Assert.Equal(2, tokens.Count);

            Assert.Equal("STRING \"string\" string", queue.Dequeue().ToString());
            Assert.Equal("EOF  null", queue.Dequeue().ToString());

        }

        [Fact]
        public void TestTokenizeStringsStringLiteral()
        {
            //Arrange
            string input = "\"foo baz\"";
            Tokenizer tokenizer = new(input);

            //Act 
            List<Token> tokens = tokenizer.Tokenize();

            //Assert
            Queue<Token> queue = new(tokens);
            Assert.Equal(2, tokens.Count);

            Assert.Equal("STRING \"foo baz\" foo baz", queue.Dequeue().ToString());
            Assert.Equal("EOF  null", queue.Dequeue().ToString());

        }
    }
}
