

namespace Nox.Tests.TestTokenizer
{
    public class Punctuators
    {
        [Fact]
        public void TestTokenizeParens()
        {
            //Arrange
            string input = "(()";
            Tokenizer tokenizer = new(input);

            //Act 
            List<Token> tokens = tokenizer.Tokenize();

            //Assert
            Queue<Token> queue = new(tokens);
            Assert.Equal(4, tokens.Count);

            Assert.Equal("LEFT_PAREN ( null", queue.Dequeue().ToString());
            Assert.Equal("LEFT_PAREN ( null", queue.Dequeue().ToString());
            Assert.Equal("RIGHT_PAREN ) null", queue.Dequeue().ToString());
            Assert.Equal("EOF  null", queue.Dequeue().ToString());
        }

        [Fact]
        public void TestTokenizeBraces()
        {
            //Arrange
            string input = "{{}}";
            Tokenizer tokenizer = new(input);

            //Act 
            List<Token> tokens = tokenizer.Tokenize();

            //Assert
            Queue<Token> queue = new(tokens);
            Assert.Equal(5, tokens.Count);

            Assert.Equal("LEFT_BRACE { null", queue.Dequeue().ToString());
            Assert.Equal("LEFT_BRACE { null", queue.Dequeue().ToString());
            Assert.Equal("RIGHT_BRACE } null", queue.Dequeue().ToString());
            Assert.Equal("RIGHT_BRACE } null", queue.Dequeue().ToString());
            Assert.Equal("EOF  null", queue.Dequeue().ToString());
        }

        [Fact]
        public void TestTokenizeOthersingleCharTokens()
        {
            //Arrange
            string input = "({*.,+*})";
            Tokenizer tokenizer = new(input);

            //Act 
            List<Token> tokens = tokenizer.Tokenize();

            //Assert
            Queue<Token> queue = new(tokens);
            Assert.Equal(10, tokens.Count);

            Assert.Equal("LEFT_PAREN ( null", queue.Dequeue().ToString());
            Assert.Equal("LEFT_BRACE { null", queue.Dequeue().ToString());
            Assert.Equal("STAR * null", queue.Dequeue().ToString());
            Assert.Equal("DOT . null", queue.Dequeue().ToString());
            Assert.Equal("COMMA , null", queue.Dequeue().ToString());
            Assert.Equal("PLUS + null", queue.Dequeue().ToString());
            Assert.Equal("STAR * null", queue.Dequeue().ToString());
            Assert.Equal("RIGHT_BRACE } null", queue.Dequeue().ToString());
            Assert.Equal("RIGHT_PAREN ) null", queue.Dequeue().ToString());
            Assert.Equal("EOF  null", queue.Dequeue().ToString());
        }

        [Fact]
        public void TestTokenizeEquals()
        {
            //Arrange
            string input = "!===<=>=!=";
            Tokenizer tokenizer = new(input);

            //Act 
            List<Token> tokens = tokenizer.Tokenize();

            //Assert
            Queue<Token> queue = new(tokens);
            Assert.Equal(6, tokens.Count);

            Assert.Equal("BANG_EQUAL != null", queue.Dequeue().ToString());
            Assert.Equal("EQUAL_EQUAL == null", queue.Dequeue().ToString());
            Assert.Equal("LESS_EQUAL <= null", queue.Dequeue().ToString());
            Assert.Equal("GREATER_EQUAL >= null", queue.Dequeue().ToString());
            Assert.Equal("BANG_EQUAL != null", queue.Dequeue().ToString());
            Assert.Equal("EOF  null", queue.Dequeue().ToString());
        }

        [Fact]
        public void TestTokenizeOthers()
        {
            //Arrange
            string input = "<>/.";
            Tokenizer tokenizer = new(input);

            //Act 
            List<Token> tokens = tokenizer.Tokenize();

            //Assert
            Queue<Token> queue = new(tokens);
            Assert.Equal(5, tokens.Count);

            Assert.Equal("LESS < null", queue.Dequeue().ToString());
            Assert.Equal("GREATER > null", queue.Dequeue().ToString());
            Assert.Equal("SLASH / null", queue.Dequeue().ToString());
            Assert.Equal("DOT . null", queue.Dequeue().ToString());
            Assert.Equal("EOF  null", queue.Dequeue().ToString());
        }

        [Fact]
        public void TestTokenizePunctuators()
        {
            //Arrange
            string input = "(){};,+-*!===<=>=!=<>/.";
            Tokenizer tokenizer = new(input);

            //Act 
            List<Token> tokens = tokenizer.Tokenize();

            //Assert
            Queue<Token> queue = new(tokens);
            Assert.Equal(19, tokens.Count);

            Assert.Equal("LEFT_PAREN ( null", queue.Dequeue().ToString());
            Assert.Equal("RIGHT_PAREN ) null", queue.Dequeue().ToString());
            Assert.Equal("LEFT_BRACE { null", queue.Dequeue().ToString());
            Assert.Equal("RIGHT_BRACE } null", queue.Dequeue().ToString());
            Assert.Equal("SEMICOLON ; null", queue.Dequeue().ToString());
            Assert.Equal("COMMA , null", queue.Dequeue().ToString());
            Assert.Equal("PLUS + null", queue.Dequeue().ToString());
            Assert.Equal("MINUS - null", queue.Dequeue().ToString());
            Assert.Equal("STAR * null", queue.Dequeue().ToString());
            Assert.Equal("BANG_EQUAL != null", queue.Dequeue().ToString());
            Assert.Equal("EQUAL_EQUAL == null", queue.Dequeue().ToString());
            Assert.Equal("LESS_EQUAL <= null", queue.Dequeue().ToString());
            Assert.Equal("GREATER_EQUAL >= null", queue.Dequeue().ToString());
            Assert.Equal("BANG_EQUAL != null", queue.Dequeue().ToString());
            Assert.Equal("LESS < null", queue.Dequeue().ToString());
            Assert.Equal("GREATER > null", queue.Dequeue().ToString());
            Assert.Equal("SLASH / null", queue.Dequeue().ToString());
            Assert.Equal("DOT . null", queue.Dequeue().ToString());
            Assert.Equal("EOF  null", queue.Dequeue().ToString());

        }

        [Fact]
        public void TestTokenizeAssignmentAndEqualityOperators()
        {
            //Arrange
            string input = "={===}";
            Tokenizer tokenizer = new(input);

            //Act 
            List<Token> tokens = tokenizer.Tokenize();

            //Assert
            Queue<Token> queue = new(tokens);
            Assert.Equal(6, tokens.Count);

            Assert.Equal("EQUAL = null", queue.Dequeue().ToString());
            Assert.Equal("LEFT_BRACE { null", queue.Dequeue().ToString());
            Assert.Equal("EQUAL_EQUAL == null", queue.Dequeue().ToString());
            Assert.Equal("EQUAL = null", queue.Dequeue().ToString());
            Assert.Equal("RIGHT_BRACE } null", queue.Dequeue().ToString());
            Assert.Equal("EOF  null", queue.Dequeue().ToString());
        }

        [Fact]
        public void TestTokenizeNegationAndInequalityOperators()
        {
            //Arrange
            string input = "!!===";
            Tokenizer tokenizer = new(input);

            //Act 
            List<Token> tokens = tokenizer.Tokenize();

            //Assert
            Queue<Token> queue = new(tokens);
            Assert.Equal(4, tokens.Count);

            Assert.Equal("BANG ! null", queue.Dequeue().ToString());
            Assert.Equal("BANG_EQUAL != null", queue.Dequeue().ToString());
            Assert.Equal("EQUAL_EQUAL == null", queue.Dequeue().ToString());
            Assert.Equal("EOF  null", queue.Dequeue().ToString());
        }

        [Fact]
        public void TestTokenizeRelationalOperators()
        {
            //Arrange
            string input = "<<=>>=";
            Tokenizer tokenizer = new(input);

            //Act 
            List<Token> tokens = tokenizer.Tokenize();

            //Assert
            Queue<Token> queue = new(tokens);
            Assert.Equal(5, tokens.Count);

            Assert.Equal("LESS < null", queue.Dequeue().ToString());
            Assert.Equal("LESS_EQUAL <= null", queue.Dequeue().ToString());
            Assert.Equal("GREATER > null", queue.Dequeue().ToString());
            Assert.Equal("GREATER_EQUAL >= null", queue.Dequeue().ToString());
            Assert.Equal("EOF  null", queue.Dequeue().ToString());
        }

        [Fact]
        public void TestTokenizeDivisionOperatorAndComments()
        {
            //Arrange
            string input = "()// Comment";
            Tokenizer tokenizer = new(input);

            //Act 
            List<Token> tokens = tokenizer.Tokenize();

            //Assert
            Queue<Token> queue = new(tokens);
            Assert.Equal(3, tokens.Count);

            Assert.Equal("LEFT_PAREN ( null", queue.Dequeue().ToString());
            Assert.Equal("RIGHT_PAREN ) null", queue.Dequeue().ToString());
            Assert.Equal("EOF  null", queue.Dequeue().ToString());
        }

    }
}
