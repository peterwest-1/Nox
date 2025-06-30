

namespace Nox.Tests.TestTokenizer
{
    public class Keywords
    {
        [Fact]
        public void TestKeywords()
        {
            //Arrange
            string input = "and class else false for fun if nil or return super this true var while";
            Tokenizer tokenizer = new(input);

            //Act 
            List<Token> tokens = tokenizer.Tokenize();

            //Assert
            Queue<Token> queue = new(tokens);
            Assert.Equal(16, tokens.Count);

            Assert.Equal("AND and null", queue.Dequeue().ToString());
            Assert.Equal("CLASS class null", queue.Dequeue().ToString());
            Assert.Equal("ELSE else null", queue.Dequeue().ToString());
            Assert.Equal("FALSE false null", queue.Dequeue().ToString());
            Assert.Equal("FOR for null", queue.Dequeue().ToString());
            Assert.Equal("FUN fun null", queue.Dequeue().ToString());
            Assert.Equal("IF if null", queue.Dequeue().ToString());
            Assert.Equal("NIL nil null", queue.Dequeue().ToString());
            Assert.Equal("OR or null", queue.Dequeue().ToString());
            Assert.Equal("RETURN return null", queue.Dequeue().ToString());
            Assert.Equal("SUPER super null", queue.Dequeue().ToString());
            Assert.Equal("THIS this null", queue.Dequeue().ToString());
            Assert.Equal("TRUE true null", queue.Dequeue().ToString());
            Assert.Equal("VAR var null", queue.Dequeue().ToString());
            Assert.Equal("WHILE while null", queue.Dequeue().ToString());
            Assert.Equal("EOF  null", queue.Dequeue().ToString());

        }

    }
}
