

namespace Nox.Tests.TestTokenizer
{
    public class Punctuators
    {
        [Fact]
        public void TestParens()
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
        public void TestBraces()
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
        public void TestOthersingleCharTokens()
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
        public void TestEquals()
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
        public void TestOthers()
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
        public void TestPunctuators()
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
        public void TestAssignmentAndEqualityOperators()
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
        public void TestNegationAndInequalityOperators()
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
        public void TestRelationalOperators()
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
        public void TestDivisionOperatorAndComments()
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
