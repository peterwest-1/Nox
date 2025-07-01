

namespace Nox.Tests.TestTokenizer
{
    public class Booleans
    {
        [Fact]
        public void TestTokenizeIdentifiers()
        {
            //Arrange
            string input = "andy formless fo _ _123 _abc ab123\r\nabcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890_";
            Tokenizer tokenizer = new(input);

            //Act 
            List<Token> tokens = tokenizer.Tokenize();

            //Assert
            Queue<Token> queue = new(tokens);
            Assert.Equal(9, tokens.Count);

            Assert.Equal("IDENTIFIER andy null", queue.Dequeue().ToString());
            Assert.Equal("IDENTIFIER formless null", queue.Dequeue().ToString());
            Assert.Equal("IDENTIFIER fo null", queue.Dequeue().ToString());
            Assert.Equal("IDENTIFIER _ null", queue.Dequeue().ToString());
            Assert.Equal("IDENTIFIER _123 null", queue.Dequeue().ToString());
            Assert.Equal("IDENTIFIER _abc null", queue.Dequeue().ToString());
            Assert.Equal("IDENTIFIER ab123 null", queue.Dequeue().ToString());
            Assert.Equal("IDENTIFIER abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890_ null", queue.Dequeue().ToString());
            Assert.Equal("EOF  null", queue.Dequeue().ToString());
        }

        [Fact]
        public void TestTokenizeIdentifiersOne()
        {
            //Arrange
            string input = "foo bar _hello";
            Tokenizer tokenizer = new(input);

            //Act 
            List<Token> tokens = tokenizer.Tokenize();

            //Assert
            Queue<Token> queue = new(tokens);
            Assert.Equal(4, tokens.Count);

            Assert.Equal("IDENTIFIER foo null", queue.Dequeue().ToString());
            Assert.Equal("IDENTIFIER bar null", queue.Dequeue().ToString());
            Assert.Equal("IDENTIFIER _hello null", queue.Dequeue().ToString());
            Assert.Equal("EOF  null", queue.Dequeue().ToString());
        }

    }
}
