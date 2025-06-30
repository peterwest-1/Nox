

namespace Nox.Tests.TestTokenizer
{
    public class Strings
    {
        [Fact]
        public void TestStringsAll()
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
        public void TestStringsEmpty()
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
        public void TestStringsString()
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
        public void TestStringsStringLiteral()
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
